using EmployeeInfoSystem.Data;
using EmployeeInfoSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInfoSystem.Repository
{
    public class AccountsRepository : IAccountsRepository
    {

        private readonly UserManager<AccountModel> _userManager;
        private readonly SignInManager<AccountModel> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountsRepository(UserManager<AccountModel> userManager, SignInManager<AccountModel> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }
        //Signup new account
        public async Task<IdentityResult> SignUp(SignUpModel signUp)
        {
            var user = new AccountModel()
            {
                Name = signUp.Name,
                Email = signUp.Email,
                DOB = signUp.DOB,
                Sex = signUp.Sex,
                UserName = signUp.Email
            };
            
            return await _userManager.CreateAsync(user, signUp.Password);
        }

        public async Task<AccountWithTokenModel> LogIn(SignInModel signInModel)
        {
            var result = await _signInManager.PasswordSignInAsync(signInModel.Email, signInModel.Password, false, false);
            if (!result.Succeeded)
            {
                return null;
            }
            var authClaims = new List<Claim> {
                new Claim(ClaimTypes.Email,signInModel.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var authSignKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSignKey,SecurityAlgorithms.HmacSha256Signature)
                ) ;
            AccountWithTokenModel account = new AccountWithTokenModel();
            account.Email = signInModel.Email;
            account.Token = new JwtSecurityTokenHandler().WriteToken(token);
            return account;

        }
        public async Task<AccountModel> GetAccountById(string email)
        {  // Fetch account by id
            return await _userManager.FindByEmailAsync(email);
        }
        ////Get information of all accounts
        //public async Task<List<AccountModel>> GetAllAccounts()
        //{
        //    var records = await _context.Account.Select(x => new AccountModel()
        //    {
        //        Id = x.Id,
        //        Name = x.Name,
        //        DOB = x.DOB,
        //        Sex = x.Sex,
        //        PasswordCriteria = x.PasswordCriteria,
        //        Password = x.Password
        //    }).ToListAsync();

        //    return records;
        //}
        ////Fetch account using id
        //public async Task<AccountModel> GetAccountById(int id)
        //{
        //    var records = await _context.Account.Where(x => x.Id == id).Select(x => new AccountModel()
        //    {
        //        Id = x.Id,
        //        Name = x.Name,
        //        DOB = x.DOB,
        //        Sex = x.Sex,
        //        PasswordCriteria = x.PasswordCriteria,
        //        Password = x.Password

        //    }).FirstOrDefaultAsync();

        //    return records;
        //}

        ////Add new account
        //public async Task<int> AddAccount(AccountModel accountModel)
        //{
        //    var account = new Account()
        //    {
        //        Id = accountModel.Id,
        //        Name = accountModel.Name,
        //        DOB = accountModel.DOB,
        //        Sex = accountModel.Sex,
        //        PasswordCriteria = accountModel.PasswordCriteria,
        //        Password = accountModel.Password
        //    };
        //    _context.Account.Add(account);
        //    await _context.SaveChangesAsync();
        //    return account.Id;
        //}
        ////Update account using id
        //public async Task<bool> UpdateAccountById(int id, AccountModel accountModel)
        //{

        //    var account = new Account()
        //    {
        //        Id = accountModel.Id,
        //        Name = accountModel.Name,
        //        DOB = accountModel.DOB,
        //        Sex = accountModel.Sex,
        //        PasswordCriteria = accountModel.PasswordCriteria,
        //        Password = accountModel.Password
        //    };
        //    _context.Account.Update(account);
        //    await _context.SaveChangesAsync();
        //    return true;
        //}
        ////Delete account using id
        //public async Task<bool> DeleteAccountById(int id)
        //{
        //    var account = new Account()
        //    {
        //        Id = id
        //    };

        //    _context.Account.Remove(account);
        //    await _context.SaveChangesAsync();
        //    return true;
        //}

    }
}
