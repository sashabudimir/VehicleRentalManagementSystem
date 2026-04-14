using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleRentalManagementSystem.Data;

namespace VehicleRentalManagementSystem.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            int totalVehicles = 0;
            int availableVehicles = 0;
            int unavailableVehicles = 0;
            int totalCustomers = 0;
            int totalReservations = 0;
            int totalBillings = 0;
            decimal totalRevenue = 0;

            try { totalVehicles = _context.Vehicles.Count(); } catch { }
            try { availableVehicles = _context.Vehicles.Count(v => v.IsAvailable); } catch { }
            try { unavailableVehicles = _context.Vehicles.Count(v => !v.IsAvailable); } catch { }
            try { totalCustomers = _context.Customers.Count(); } catch { }
            try { totalReservations = _context.Reservations.Count(); } catch { }
            try { totalBillings = _context.Billings.Count(); } catch { }
            try { totalRevenue = _context.Billings.Sum(b => (decimal?)b.TotalAmount) ?? 0; } catch { }

            ViewBag.TotalVehicles = totalVehicles;
            ViewBag.AvailableVehicles = availableVehicles;
            ViewBag.UnavailableVehicles = unavailableVehicles;
            ViewBag.TotalCustomers = totalCustomers;
            ViewBag.TotalReservations = totalReservations;
            ViewBag.TotalBillings = totalBillings;
            ViewBag.TotalRevenue = totalRevenue;

            return View();
        }
    }
}