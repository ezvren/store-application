using Moq;
using Store.Application.Services.Implementation;
using Store.Application.Helpers;
using Store.Infrastructure.Repositories.Contract;
using Store.Core.Entities;

namespace Store.Application.Test
{
    public class OperationServiceTests
    {
        private readonly Mock<IOperationRepository> _operationRepositoryMock;
        private readonly OperationService _service;
        private readonly OperationHelper _operationHelper;

        public OperationServiceTests()
        {
            _operationRepositoryMock = new Mock<IOperationRepository>();
            _operationHelper = new OperationHelper();
            _service = new OperationService(_operationRepositoryMock.Object, _operationHelper);
        }

        public static IEnumerable<object[]> InvalidAmountsTestData =>
        [
                [null, 100.00m, "Total amount and received amount cannot be null."],
                [100.00m, null, "Total amount and received amount cannot be null."],
                [0.00m, 100.00m, "Amounts must be greater than zero."],
                [100.00m, 0.00m, "Amounts must be greater than zero."],
                [-10.00m, 100.00m, "Amounts must be greater than zero."],
                [200.00m, 150.00m, "Received amount cannot be less than total amount."],
                [10.05m, 20.00m, "Both amounts must be multiples of 0.10."]
        ];

        [Theory]
        [MemberData(nameof(InvalidAmountsTestData))]
        public async Task SaveOperation_InvalidAmounts_ReturnsErrorWithoutSaving(decimal? totalAmount, decimal? receivedAmount, string expectedMessage)
        {
            var result = await _service.SaveOperation(totalAmount, receivedAmount, Guid.NewGuid());

            Assert.False(result.Success);
            Assert.Equal(expectedMessage, result.Message);

            _operationRepositoryMock.Verify(repository => repository.AddAsync(It.IsAny<Operation>(), It.IsAny<CancellationToken>()), Times.Never);
            _operationRepositoryMock.Verify(repository => repository.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task SaveOperation_ExactPayment_SavesOperationWithoutBreakdowns()
        {
            Operation? savedOperation = null;

            _operationRepositoryMock
                .Setup(repository => repository.AddAsync(
                    It.IsAny<Operation>(), 
                    It.IsAny<CancellationToken>()))
                 .Callback<Operation, CancellationToken>(
                    (operation, cancellationToken) => savedOperation = operation)
                 .Returns(Task.CompletedTask);

            _operationRepositoryMock.Setup(repository => repository.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _service.SaveOperation(80.00m, 80.00m, Guid.NewGuid());

            Assert.True(result.Success);
            Assert.Equal("Operation saved successfully. No change is required.", result.Message);
            Assert.NotNull(savedOperation);
            Assert.Equal(80.00m, savedOperation.TotalAmount);
            Assert.Equal(80.00m, savedOperation.ReceivedAmount);
            Assert.Equal(0.00m, savedOperation.ChangeAmount);
            Assert.Empty(savedOperation.OperationBreakdowns);

            _operationRepositoryMock.Verify(repository => repository.AddAsync(It.IsAny<Operation>(), It.IsAny<CancellationToken>()), Times.Once);
            _operationRepositoryMock.Verify(repository => repository.SaveChangesAsync(), Times.Once);
        }

        public static TheoryData<decimal, decimal, decimal> ChangeCases => new()
        {
            { 50.00m, 100.00m, 50.00m },
            { 125.50m, 200.00m, 74.50m },
            { 349.70m, 500.00m, 150.30m }
        };

        [Theory]
        [MemberData(nameof(ChangeCases))]
        public async Task SaveOperation_WithChange_SavesCorrectChangeAmount(decimal totalAmount, decimal receivedAmount, decimal expectedChange)
        {
            Operation? savedOperation = null;

            _operationRepositoryMock
                .Setup(repository => repository.AddAsync(
                    It.IsAny<Operation>(),
                    It.IsAny<CancellationToken>()))
                .Callback<Operation, CancellationToken>(
                    (operation, cancellationToken) => savedOperation = operation)
                .Returns(Task.CompletedTask);

            _operationRepositoryMock.Setup(repository => repository.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _service.SaveOperation(totalAmount, receivedAmount, Guid.NewGuid());

            Assert.True(result.Success);
            Assert.Equal("Operation saved successfully.", result.Message);
            Assert.NotNull(savedOperation);
            Assert.Equal(totalAmount, savedOperation.TotalAmount);
            Assert.Equal(receivedAmount, savedOperation.ReceivedAmount);
            Assert.Equal(expectedChange, savedOperation.ChangeAmount);
            Assert.NotEmpty(savedOperation.OperationBreakdowns);
            Assert.Equal(expectedChange, savedOperation.OperationBreakdowns.Sum(breakdown => breakdown.DenominationValue * breakdown.Quantity));

            _operationRepositoryMock.Verify(repository => repository.AddAsync( It.IsAny<Operation>(), It.IsAny<CancellationToken>()), Times.Once);
            _operationRepositoryMock.Verify(repository => repository.SaveChangesAsync(), Times.Once);
        }
    }
}