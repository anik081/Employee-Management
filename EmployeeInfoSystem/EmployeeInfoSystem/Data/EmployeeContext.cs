using EmployeeInfoSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeInfoSystem.Data
{
    public class EmployeeContext: IdentityDbContext<AccountModel>
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options): base(options)
        {

        }
        public DbSet<Employee> Employee { get; set; }
    }
}
