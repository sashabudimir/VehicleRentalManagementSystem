using System.ComponentModel.DataAnnotations;

namespace VehicleRentalManagementSystem.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required]
        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }

        [Required]
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        public string Status { get; set; } = "Active";
    }
}