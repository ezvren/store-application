using Microsoft.AspNetCore.Identity;
using Store.Core.Entities;
using Store.Infrastructure.Repositories.Contract;

namespace Store.Infrastructure.Repositories.Implementation
{
    public class AuthRepository(UserManager<Cashier> userManager, SignInManager<Cashier> signInManager) : IAuthRepository
    {
        public async Task<IdentityResult> CreateUser(string userName, string email, string password)
        {
            var user = new Cashier
            {
                UserName = userName,
                Email = email
            };

            return await userManager.CreateAsync(user, password);
        }

        public async Task<bool> UserExists(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            return user is not null;
        }

        public async Task<Cashier?> GetUserByEmail(string email)
        {
            return await userManager.FindByEmailAsync(email);
        }
    }
}
