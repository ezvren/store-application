using Store.Application.DTOs;
using Store.Application.Helpers;
using Store.Application.Services.Contract;
using Store.Core.Entities;
using Store.Infrastructure.Repositories.Contract;

namespace Store.Application.Services.Implementation
{
    public class OperationService(IOperationRepository operationRepository, OperationHelper operationHelper) : IOperationService
    {

        public async Task<ResponseDTO> SaveOperation(decimal? totalAmount, decimal? receivedAmount)
        {
            ResponseDTO responseHelper = new();

            if (totalAmount is null || receivedAmount is null)
            {
                responseHelper.Success = false;
                responseHelper.Message = "Total amount and received amount cannot be null.";

            } else if (totalAmount.Value <= 0 || receivedAmount.Value <= 0)
            {
                responseHelper.Success = false;
                responseHelper.Message = "Amounts must be greater than zero.";
            } else if (receivedAmount.Value < totalAmount.Value)
            {
                responseHelper.Success = false;
                responseHelper.Message = "Received amount cannot be less than total amount.";

            } else if (receivedAmount.Value % 0.10m != 0 || totalAmount.Value % 0.10m != 0)
            {
                responseHelper.Success = false;
                responseHelper.Message = "Both amounts must be multiples of 0.10.";

            } else
            {
                decimal total = totalAmount.Value;
                decimal received = receivedAmount.Value;
                decimal change = received - total;

                Operation operation = new()
                {
                    CashierId = Guid.NewGuid(),
                    Date = DateTime.UtcNow,
                    TotalAmount = total,
                    ReceivedAmount = received,
                    ChangeAmount = change
                };

                if (change > 0)
                {
                    List<DenominationBreakdownDTO> cashOperation = await operationHelper.CreateCashOperation(change);
                    operation.OperationBreakdowns = [.. cashOperation.Select(b => new OperationBreakdown
                    {
                        DenominationValue = b.DenominationValue,
                        Quantity = b.Quantity,
                        DenominationKind = b.DenominationKind
                    })];
                }

                await operationRepository.AddAsync(operation);
                await operationRepository.SaveChangesAsync();


                responseHelper.Success = true;
                responseHelper.Message = change == 0 ? "Operation saved successfully. No change is required." : "Operation saved successfully.";
            }
            return responseHelper;
        }
    }
}
