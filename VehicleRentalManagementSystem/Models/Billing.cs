using System.ComponentModel.DataAnnotations;

namespace VehicleRentalManagementSystem.Models
{
    public class Billing
    {
        public int Id { get; set; }

        [Required]
        public int ReservationId { get; set; }
        public Reservation? Reservation { get; set; }

        public decimal BaseAmount { get; set; }
        public decimal Tax { get; set; }
        public decimal AdditionalCharges { get; set; }
        public decimal TotalAmount { get; set; }

        public DateTime BillingDate { get; set; } = DateTime.Now;
    }
}