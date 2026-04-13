using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VehicleRentalManagementSystem.Data;
using VehicleRentalManagementSystem.Models;

namespace VehicleRentalManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            ViewBag.VehicleCount = _context.Vehicles.Count();
            ViewBag.AvailableVehicles = _context.Vehicles.Count(v => v.IsAvailable);
            ViewBag.CustomerCount = _context.Customers.Count();
            ViewBag.ReservationCount = _context.Reservations.Count();
            ViewBag.BillingCount = _context.Billings.Count();
            ViewBag.TotalRevenue = _context.Billings.Sum(b => (decimal?)b.TotalAmount) ?? 0;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}