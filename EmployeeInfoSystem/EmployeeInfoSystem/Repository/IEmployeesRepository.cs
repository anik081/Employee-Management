using EmployeeInfoSystem.Data;
using EmployeeInfoSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeInfoSystem.Repository
{
    public interface IEmployeesRepository
    {
        Task<List<EmployeeModel>> GetAllEmployee();
        Task<EmployeeModel> GetEmployeesById(int id);
        Task<List<EmployeeModel>> GetEmployeesByMovie(string movieName);
        Task<int> AddEmployee(EmployeeModel employeeModel);
        Task<bool> UpdateEmployeeById(int id, EmployeeModel employeeModel);
        Task<bool> DeleteEmployeeById(int id);


    }
}
