using Store.Application.DTOs;
using System.Security.Claims;

namespace Store.Application.Services.Contract
{
    public interface IOperationService
    {
        Task<ResponseDTO> SaveOperation(decimal? totalAmount, decimal? receivedAmount, ClaimsPrincipal user);
        Task<ResponseOperationDTO> SearchOperation(Guid id);
    }
}
