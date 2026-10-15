using Microsoft.AspNetCore.Identity;
using Store.Application.DTOs;
using Store.Application.Helpers;
using Store.Application.Services.Contract;
using Store.Core.Entities;
using Store.Infrastructure.Repositories.Contract;
using System.Security.Claims;

namespace Store.Application.Services.Implementation
{
    public class OperationService(IOperationRepository operationRepository, OperationHelper operationHelper) : IOperationService
    {
        public async Task<List<ResponseOperationHistoryDTO>> HistoryOperationForUser(Guid id)
        {
            var operations = await operationRepository.GetOperationByUser(id);

            if (operations is null || operations.Count == 0) return [];

            return operations.GroupBy(o => o.Date.Date)
                .Select(group => new ResponseOperationHistoryDTO
                {
                    Date = group.Key,
                    Operations = group.Select(o => new ResponseOperationDTO
                    {
                        Id = o.Id,
                        TotalAmount = o.TotalAmount,
                        ReceivedAmount = o.ReceivedAmount,
                        ChangeAmount = o.ChangeAmount,
                        ChangeBreakdown = o.OperationBreakdowns?
                            .Select(b => new DenominationBreakdownDTO
                            {
                                DenominationValue = b.DenominationValue,
                                Quantity = b.Quantity,
                                DenominationKind = b.DenominationKind
                            })
                            .OrderByDescending(b => b.DenominationValue)
                            .ToList()
                    }).ToList()
                })
                .ToList();
        }

        public async Task<ResponseOperationDTO> SearchOperation(Guid id)
        {
            Operation? operation = await operationRepository.GetByIdAsync(id);

            if (operation is null) return null;

            return new ResponseOperationDTO
            {
                TotalAmount = operation.TotalAmount,
                ReceivedAmount = operation.ReceivedAmount,
                ChangeAmount = operation.ChangeAmount,
                ChangeBreakdown = operation.OperationBreakdowns?.Select(b => new DenominationBreakdownDTO
                {
                    DenominationValue = b.DenominationValue,
                    Quantity = b.Quantity,
                    DenominationKind = b.DenominationKind
                }).OrderByDescending(x => x.DenominationValue).ToList()
            };
        }

        public async Task<ResponseDTO> SaveOperation(decimal? totalAmount, decimal? receivedAmount, Guid idUser)
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
                    CashierId = idUser,
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
                responseHelper.Data = new ResponseOperationDTO
                {
                    TotalAmount = total,
                    ReceivedAmount = received,
                    ChangeAmount = change,
                    ChangeBreakdown = operation.OperationBreakdowns?.Select(b => new DenominationBreakdownDTO
                    {
                        DenominationValue = b.DenominationValue,
                        Quantity = b.Quantity,
                        DenominationKind = b.DenominationKind
                    }).OrderByDescending(x => x.DenominationValue).ToList()
                };
            }
            return responseHelper;
        }
    }
}
