using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize (Roles = "Admin")]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeDbContext _dbContext;

        public EmployeeController(EmployeeDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        
        [HttpGet("AllEmployees")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployees()
        {
            var employees =  await _dbContext.Employees
                                    .Where(e => !e.IsDeleted).ToListAsync();
            return Ok(employees);                                 
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee =await _dbContext.Employees.FindAsync(id);

            if (employee == null)
                return NotFound("The Employee is not Found");
            if (employee.IsDeleted)
                return BadRequest("The Employee is Deleted From The System");

            return Ok(employee);
        }

        [HttpGet("DeletedEmployees")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetDeletedEmployees()
        {
            var deletedEmployees =  await _dbContext.Employees
                                    .Where(e => e.IsDeleted).ToListAsync();
            if (deletedEmployees == null)
                return NotFound("No DEleted Employees yet .");

            return Ok(deletedEmployees);
        }

        [HttpPost("AddEmployee")]
        public async Task<ActionResult> AddEmployee(EmployeeDTO employee)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

          
            var Addemployee = new Employee
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                JobTitle = employee.JobTitle,
                HireDate = employee.HireDate,
                IsDeleted = false
            };

            _dbContext.Employees.Add(Addemployee);
            await _dbContext.SaveChangesAsync();
            return Ok("Employee Added Successfully");
        }

        [HttpPost("UnDeleteEmployee/{employeeId}")]
        public async Task<ActionResult> UnDeleteEmployee(int employeeId)
        {
            var employee = await _dbContext.Employees.FindAsync(employeeId);

            if (employee == null)
            {
                return NotFound($"Employee with ID {employeeId} not found.");
            }

            if (!employee.IsDeleted)
            {
                return BadRequest("The Employee is not deleted to undelete him .");
            }

            // Mark the employee as undeleted
            employee.IsDeleted = false;

            try
            {
                await _dbContext.SaveChangesAsync();
                return Ok("Employee is retrieved successfully.");
            }
            catch (Exception ex)
            {
                // Handle exceptions, such as database errors
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("EditEmployee/{employeeId}")]
        public async Task<ActionResult> UpdateEmployee(int employeeId, EmployeeDTO employeeDto)
        {
           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _dbContext.Employees.FindAsync(employeeId);

            if (employee == null)
            {
                return NotFound($"Employee with ID {employeeId} not found.");
            }

            // Update employee properties
            employee.FirstName = employeeDto.FirstName;
            employee.LastName = employeeDto.LastName;
            employee.Email = employeeDto.Email;
            employee.PhoneNumber = employeeDto.PhoneNumber;
            employee.JobTitle = employeeDto.JobTitle;
            employee.HireDate = employeeDto.HireDate.Date; // Make sure to set only the date

            try
            {
                await _dbContext.SaveChangesAsync();
                return Ok("Employee updated successfully.");
            }
            catch (Exception ex)
            {
                // Handle exceptions, such as database errors
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("DeleteEmployee/{employeeId}")]
        public async Task<ActionResult> DeleteEmployee(int employeeId)
        {
            var employee = await _dbContext.Employees.FindAsync(employeeId);

            if (employee == null)
            {
                return NotFound($"Employee with ID {employeeId} not found.");
            }

            // Mark the employee as deleted
            employee.IsDeleted = true;

            try
            {
                await _dbContext.SaveChangesAsync();
                return Ok("Employee is deleted successfully.");
            }
            catch (Exception ex)
            {
                // Handle exceptions, such as database errors
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



    }
}
