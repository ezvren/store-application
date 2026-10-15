using Microsoft.AspNetCore.Identity;

namespace Store.Application.Services.Contract
{
    public interface IAuthService
    {
        Task<IdentityResult> Register(string email, string password);
        Task<bool> Login(string email, string password);
        Task Logout();
    }
}
