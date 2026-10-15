using Store.Application.DTOs;
using System.Security.Claims;

namespace Store.Application.Services.Contract
{
    public interface IOperationService
    {
        Task<List<ResponseOperationHistoryDTO>> HistoryOperationForUser(Guid id);
        Task<ResponseDTO> SaveOperation(decimal? totalAmount, decimal? receivedAmount, Guid idUser);
        Task<ResponseOperationDTO> SearchOperation(Guid id);
    }
}
