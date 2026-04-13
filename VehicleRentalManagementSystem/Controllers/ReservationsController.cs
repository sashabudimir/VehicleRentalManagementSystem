using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VehicleRentalManagementSystem.Data;
using VehicleRentalManagementSystem.Models;

namespace VehicleRentalManagementSystem.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var reservations = _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.Vehicle);

            return View(await reservations.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var reservation = await _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reservation == null) return NotFound();

            return View(reservation);
        }

        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reservation reservation)
        {
            if (reservation.EndDate < reservation.StartDate)
            {
                ModelState.AddModelError("", "End date cannot be earlier than start date.");
            }

            if (ModelState.IsValid)
            {
                var vehicle = await _context.Vehicles.FindAsync(reservation.VehicleId);
                if (vehicle != null)
                {
                    vehicle.IsAvailable = false;
                }

                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            LoadDropdowns(reservation.CustomerId, reservation.VehicleId);
            return View(reservation);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null) return NotFound();

            LoadDropdowns(reservation.CustomerId, reservation.VehicleId);
            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Reservation reservation)
        {
            if (id != reservation.Id) return NotFound();

            if (reservation.EndDate < reservation.StartDate)
            {
                ModelState.AddModelError("", "End date cannot be earlier than start date.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Reservations.Any(e => e.Id == reservation.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            LoadDropdowns(reservation.CustomerId, reservation.VehicleId);
            return View(reservation);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var reservation = await _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reservation == null) return NotFound();

            return View(reservation);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);

            if (reservation != null)
            {
                var vehicle = await _context.Vehicles.FindAsync(reservation.VehicleId);
                if (vehicle != null)
                {
                    vehicle.IsAvailable = true;
                }

                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private void LoadDropdowns(int? customerId = null, int? vehicleId = null)
        {
            ViewData["CustomerId"] = new SelectList(
                _context.Customers.Select(c => new
                {
                    Id = c.Id,
                    Name = c.FirstName + " " + c.LastName
                }),
                "Id",
                "Name",
                customerId
            );

            ViewData["VehicleId"] = new SelectList(
                _context.Vehicles.Select(v => new
                {
                    Id = v.Id,
                    Name = v.Make + " " + v.Model
                }),
                "Id",
                "Name",
                vehicleId
            );
        }
    }
}