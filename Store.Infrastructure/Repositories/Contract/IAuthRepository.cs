using Microsoft.AspNetCore.Identity;
using Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Infrastructure.Repositories.Contract
{
    public interface IAuthRepository
    {
        Task<IdentityResult> CreateUser(string userName, string email, string password);
        Task<Cashier?> GetUserByEmail(string email);
        Task<bool> UserExists(string email);
    }
}
