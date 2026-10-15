using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Infrastructure.Repositories.Contract
{
    public interface IAuthRepository
    {
        Task<IdentityResult> CreateUser(string userName, string email, string password);
        Task<bool> UserExists(string email);
        Task<bool> Login(string userName, string password);
    }
}
