using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [Authorize]
        public IActionResult Dashboard()
        {
            int totalVehicles = 0;
            int availableVehicles = 0;
            int totalCustomers = 0;
            int totalReservations = 0;
            int totalBillings = 0;
            decimal totalRevenue = 0;

            try { totalVehicles = _context.Vehicles.Count(); } catch { }
            try { availableVehicles = _context.Vehicles.Count(v => v.IsAvailable); } catch { }
            try { totalCustomers = _context.Customers.Count(); } catch { }
            try { totalReservations = _context.Reservations.Count(); } catch { }
            try { totalBillings = _context.Billings.Count(); } catch { }
            try { totalRevenue = _context.Billings.Sum(b => (decimal?)b.TotalAmount) ?? 0; } catch { }

            ViewBag.TotalVehicles = totalVehicles;
            ViewBag.AvailableVehicles = availableVehicles;
            ViewBag.TotalCustomers = totalCustomers;
            ViewBag.TotalReservations = totalReservations;
            ViewBag.TotalBillings = totalBillings;
            ViewBag.TotalRevenue = totalRevenue;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}