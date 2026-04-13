using System.ComponentModel.DataAnnotations;

namespace VehicleRentalManagementSystem.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; } = "";

        [Required]
        public string LastName { get; set; } = "";

        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string PhoneNumber { get; set; } = "";

        [Required]
        public string DriverLicenseNumber { get; set; } = "";

        public string FullName => $"{FirstName} {LastName}";
    }
}