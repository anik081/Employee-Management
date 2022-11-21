using EmployeeInfoSystem.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeInfoSystem.Repository
{
    public interface IAccountsRepository
    {

        Task<IdentityResult> SignUp(SignUpModel signUp);
        Task<AccountWithTokenModel> LogIn(SignInModel signInModel);
        Task<AccountModel> GetAccountById(string userId);
    }
}
