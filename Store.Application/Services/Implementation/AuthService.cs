using Microsoft.AspNetCore.Identity;
using Store.Application.Services.Contract;
using Store.Core.Entities;
using Store.Infrastructure.Repositories.Contract;

namespace Store.Application.Services.Implementation
{
    public class AuthService(IAuthRepository authRepository, SignInManager<Cashier> signInManager) : IAuthService
    {
        public async Task<IdentityResult> Register(string email, string password)
        {
            var cashierExists = await authRepository.UserExists(email);

            if (cashierExists) return IdentityResult.Failed(new IdentityError { Description = "User already exists" });

            return await authRepository.CreateUser($"cashier_{Guid.NewGuid():N}", email, password);
        }

        public async Task<bool> Login(string email, string password)
        {
            Cashier? cashier = await authRepository.GetUserByEmail(email);

            if(cashier is null) return false;

            SignInResult result = await signInManager.PasswordSignInAsync(cashier, password, false, false);

            return result.Succeeded;
        }

        public async Task Logout()
        {
            await signInManager.SignOutAsync();
        }
    }
}