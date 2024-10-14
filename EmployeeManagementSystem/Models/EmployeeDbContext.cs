using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace EmployeeManagementSystem.Models
{
    public class EmployeeDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public EmployeeDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; } 

    }
}
