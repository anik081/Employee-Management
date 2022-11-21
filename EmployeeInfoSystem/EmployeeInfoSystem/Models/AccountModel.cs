using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeInfoSystem.Models
{
    public class AccountModel: IdentityUser
    {
        public string Name { get; set; }
        public string DOB { get; set; }
        public string Sex { get; set; }
        public string Password { get; set; }
    }
}
