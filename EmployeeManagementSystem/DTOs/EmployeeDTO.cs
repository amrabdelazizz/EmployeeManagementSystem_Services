
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace EmployeeManagementSystem.DTOs
{
    // used when adding a new employee 
    public class EmployeeDTO
    {
        [Required]
        [StringLength(20, ErrorMessage = "First Name cannot be longer than 20 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(20, ErrorMessage = "Last Name cannot be longer than 20 characters.")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^(011|012|010|015)\d{8}$", ErrorMessage = "Phone number must be 11 digits and start with '011', '012', '010', or '015'.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "Job Title cannot be longer than 100 characters.")]
        public string JobTitle { get; set; } = string.Empty;

        [Required]
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime HireDate { get; set; }
        
    }
}
