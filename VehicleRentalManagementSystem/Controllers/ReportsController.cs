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
            ViewBag.TotalVehicles = _context.Vehicles.Count();
            ViewBag.AvailableVehicles = _context.Vehicles.Count(v => v.IsAvailable);
            ViewBag.UnavailableVehicles = _context.Vehicles.Count(v => !v.IsAvailable);
            ViewBag.TotalCustomers = _context.Customers.Count();
            ViewBag.TotalReservations = _context.Reservations.Count();
            ViewBag.TotalBillings = _context.Billings.Count();
            ViewBag.TotalRevenue = _context.Billings.Sum(b => (decimal?)b.TotalAmount) ?? 0;

            return View();
        }
    }
}