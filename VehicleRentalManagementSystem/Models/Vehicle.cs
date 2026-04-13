using System.ComponentModel.DataAnnotations;

namespace VehicleRentalManagementSystem.Models
{
    public class Vehicle
    {
        public int Id { get; set; }

        [Required]
        public string Make { get; set; } = "";

        [Required]
        public string Model { get; set; } = "";

        [Required]
        public int Year { get; set; }

        [Required]
        public string Type { get; set; } = "";

        [Required]
        public string LicensePlate { get; set; } = "";

        [Required]
        [Range(1, 10000)]
        public decimal PricePerDay { get; set; }

        public bool IsAvailable { get; set; } = true;
    }
}