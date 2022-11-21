using EmployeeInfoSystem.Models;
using EmployeeInfoSystem.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeInfoSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeesRepository _employeesRepository;
        private readonly ILogger _logs;

        public EmployeeController(IEmployeesRepository employeesRepository, ILogger<IEmployeesRepository> logs)
        {
            this._employeesRepository = employeesRepository;
            _logs = logs;
        }

        #region Fetch all employees
        [HttpGet("")]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                _logs.LogInformation("Fetching all employees");
                var res = await _employeesRepository.GetAllEmployee();
                if (res.Count == 0)
                {
                    _logs.LogWarning("No employees found!");
                    return NotFound("No employee found!");
                }
                else
                {

                    _logs.LogInformation("sucessfully fetch data!");
                    return Ok(res);
                }
            }
            catch(Exception ex)
            {
                _logs.LogError(ex.Message);
                return Unauthorized(ex.Message);
            }

        }
        #endregion
        #region Search Employee using Id
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetEmployeesById([FromRoute] int Id)
        {
            try
            {
                _logs.LogInformation($"Searching employee with id:{Id}");
                var res =await _employeesRepository.GetEmployeesById(Id);
                if (res.Id == 0)
                {
                    _logs.LogWarning($"No employees found with Id: {Id}!");
                    return NotFound("No employee found!");
                }
                else
                {

                    _logs.LogInformation($"Employees fetched successfully with Id: {Id}!");
                    return Ok(res);
                }
            }
            catch(Exception ex)
            {
                _logs.LogError(ex.Message);
                return Unauthorized(ex.Message);
            }

        }
        #endregion

        #region Search user by mmovie name
        [HttpGet("Movie/{name}")]
        public async Task<IActionResult> GetEmployeesByMovie([FromRoute] string name)
        {
            try
            {
                _logs.LogInformation($"Searching employee with movie:{name}");
                var res =await _employeesRepository.GetEmployeesByMovie(name);
                if (res.Count ==0)
                {
                    _logs.LogWarning($"No employees found with movie: {name}!");
                    return NotFound("No employee found!");
                }
                else
                {

                    _logs.LogInformation($"Successfully fethed employee with movie: {name}!");
                    return Ok(res);
                }
            }
            catch(Exception ex)
            {
                _logs.LogError(ex.Message);
                return Unauthorized(ex.Message);
            }

        }
        #endregion

        #region Save new employee
        [HttpPost("")]
        public async Task<IActionResult> SaveEmployee([FromBody] EmployeeModel employee)
        {
            try
            {
                _logs.LogInformation("Saving new employee");
                var res =await _employeesRepository.AddEmployee(employee);
                if (res == 0)
                {
                    _logs.LogWarning("Could not save data!");
                    return BadRequest("Could not save data");
                }

                _logs.LogInformation("Saved employee successfully");
                return Ok(res);
            }
            catch(Exception ex)
            {
                _logs.LogError(ex.Message);
                return Unauthorized(ex.Message);
            }

        }
        #endregion


        #region Update employee information
        [HttpPut("")]
        public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeModel employee)
        {
            try
            {
                _logs.LogInformation($"Updating employee with id:{employee.Id}");
                var res =await _employeesRepository.UpdateEmployeeById(employee.Id, employee);
                if (res == false)
                {
                    _logs.LogWarning("Could not update data");
                    return BadRequest("Could not update data");
                }
                _logs.LogInformation("Successfully updated information");
                return Ok(res);
            }
            catch(Exception ex)
            {
                _logs.LogError(ex.Message);
                return Unauthorized(ex.Message);
            }

        }
        #endregion


        #region Remove employee by id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] int id)
        {
            try
            {
                _logs.LogInformation($"Deleting employee with id:{id}");
                var res =await _employeesRepository.DeleteEmployeeById(id);
                if (res == false)
                {
                    _logs.LogWarning($"Could not delete employee with id:{id}");
                    return BadRequest("Could not delete data");
                }
                _logs.LogInformation($"Successfully deleted employee with id:{id}");
                return Ok(res);
            }
            catch(Exception ex)
            {
                _logs.LogError(ex.Message);
                return Unauthorized();
            }

        }
        #endregion

    }
}
