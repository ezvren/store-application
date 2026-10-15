using Microsoft.AspNetCore.Identity;
using Store.Application.Services.Contract;
using Store.Infrastructure.Repositories.Contract;

namespace Store.Application.Services.Implementation
{
    public class AuthService(IAuthRepository authRepository) : IAuthService
    {
        public async Task<IdentityResult> CreateUser(string email, string password)
        {
            return await authRepository.CreateUser($"cashier_{Guid.NewGuid():N}", email, password);
        }

        public async Task<bool> UserExists(string email)
        {
            return await authRepository.UserExists(email);
        }

        public async Task<bool> Login(string email, string password)
        {
            return await authRepository.Login(email, password);
        }
    }
}
