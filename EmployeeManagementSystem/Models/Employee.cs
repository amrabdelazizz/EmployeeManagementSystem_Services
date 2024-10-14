

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EmployeeManagementSystem.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        
        public string FirstName { get; set; } = string.Empty;
        
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;


        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime HireDate { get; set; }
        public bool IsDeleted { get; set; }

    }
}
