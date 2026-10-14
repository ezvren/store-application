using Store.Application.Helpers;

namespace Store.Application.Test
{
    public class OperationHelperTests
    {
        private readonly OperationHelper _helper = new();

        [Theory]
        [InlineData(1500, 500.00, 3)]
        [InlineData(1500, 200.00, 7)]
        [InlineData(0.50, 0.50, 1)]
        [InlineData(5.50, 0.50, 11)]

        public void CalculateDenomination_ReturnsExpectedQuantity(decimal amount, decimal denomination, int expectedQuantity)
        {
            var result = _helper.CalculateDenomination(amount, denomination);

            Assert.NotNull(result);
            Assert.Equal(expectedQuantity, result!.Quantity);
            Assert.Equal(denomination, result.DenominationValue);
        }

        [Theory]
        [InlineData(50.00, "50:1")]
        [InlineData(100.00, "100:1")]
        [InlineData(200.00, "200:1")]
        [InlineData(300.00, "200:1, 100:1")]
        [InlineData(150.00, "100:1, 50:1")]
        [InlineData(85.50, "50:1, 20:1, 10:1, 5:1, 0.50:1")]
        [InlineData(14.50, "10:1, 1:4, 0.50:1")]
        [InlineData(300.10, "200:1, 100:1, 0.10:1")]
        [InlineData(386.80, "200:1, 100:1, 50:1, 20:1, 10:1, 5:1, 1:1, 0.50:1, 0.20:1, 0.10:1")]
        [InlineData(99.90, "50:1, 20:2, 5:1, 1:4, 0.50:1, 0.20:2")]
        [InlineData(75.00, "50:1, 20:1, 5:1")]
        [InlineData(42.60, "20:2, 1:2, 0.50:1, 0.10:1")]
        [InlineData(18.30, "10:1, 5:1, 1:3, 0.20:1, 0.10:1")]
        [InlineData(7.80, "5:1, 1:2, 0.50:1, 0.20:1, 0.10:1")]
        [InlineData(3.70, "1:3, 0.50:1, 0.20:1")]
        [InlineData(1.60, "1:1, 0.50:1, 0.10:1")]
        [InlineData(0.90, "0.50:1, 0.20:2")]
        [InlineData(0.40, "0.20:2")]
        [InlineData(0.10, "0.10:1")]
        public async Task CreateCashOperation_ReturnsExpectedDenominations(decimal change, string expectedDenominations)
        {
            var result = await _helper.CreateCashOperation(change);

            var actualDenominations = string.Join(", ", result.Select(x => $"{x.DenominationValue}:{x.Quantity}"));

            Assert.Equal(expectedDenominations, actualDenominations);
        }
    }
}