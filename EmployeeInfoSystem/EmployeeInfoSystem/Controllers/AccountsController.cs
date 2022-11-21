using EmployeeInfoSystem.Models;
using EmployeeInfoSystem.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeInfoSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsRepository _accountsRepository;
        private readonly ILogger _logger;

        public AccountsController(IAccountsRepository accountsRepository, ILogger<AccountsController> logger)
        {
            this._accountsRepository = accountsRepository;
            _logger = logger;
        }
        [HttpGet("{email}")]
        public async Task<IActionResult> GetAccountById([FromRoute]string email)
        {
            try
            {
                _logger.LogInformation("Fetching account by email");
                var result = await _accountsRepository.GetAccountById(email);
                if (result == null)
                {
                    _logger.LogWarning($"Could not get data by email:{email}");
                    return Unauthorized($"Could not get data by email:{email}");

                }
                _logger.LogInformation("Fetched account successfully");
                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return Unauthorized(ex.Message);
            } 
            
        }
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody]SignUpModel signup)
        {
            try {
                _logger.LogInformation("Siging up account");
                var result = await _accountsRepository.SignUp(signup);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Signup successfull");
                    return Ok(result.Succeeded);
                }
                else
                {
                    _logger.LogWarning(result.Errors.ToString());
                    return Unauthorized(result.Errors);
                }
            } catch(Exception ex) {
                _logger.LogError(ex.Message);
                return Unauthorized(ex.Message);
            }
           
        }
        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromBody] SignInModel signIn)
        {
            try {
                _logger.LogInformation("Signing In");
                var result = await _accountsRepository.LogIn(signIn);
                if (string.IsNullOrEmpty(result.Token))
                {
                    _logger.LogWarning("Could not generate token");
                    return Unauthorized("Could not generate token");
                }
                _logger.LogInformation("Log in successfull");
                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return Unauthorized(ex.Message);
            }
            
        }
       
    }
}
