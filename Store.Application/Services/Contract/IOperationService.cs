using Store.Application.DTOs;

namespace Store.Application.Services.Contract
{
    public interface IOperationService
    {
        Task<ResponseDTO> SaveOperation(decimal? totalAmount, decimal? receivedAmount);
    }
}
