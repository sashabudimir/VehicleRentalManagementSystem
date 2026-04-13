using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VehicleRentalManagementSystem.Data;
using VehicleRentalManagementSystem.Models;

namespace VehicleRentalManagementSystem.Controllers
{
    [Authorize]
    public class BillingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BillingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===================== INDEX =====================
        public async Task<IActionResult> Index()
        {
            var billings = _context.Billings
                .Include(b => b.Reservation)
                    .ThenInclude(r => r!.Customer)
                .Include(b => b.Reservation)
                    .ThenInclude(r => r!.Vehicle);

            return View(await billings.ToListAsync());
        }

        // ===================== DETAILS =====================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var billing = await _context.Billings
                .Include(b => b.Reservation)
                    .ThenInclude(r => r!.Customer)
                .Include(b => b.Reservation)
                    .ThenInclude(r => r!.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (billing == null) return NotFound();

            return View(billing);
        }

        // ===================== CREATE (GET) =====================
        public IActionResult Create()
        {
            ViewData["ReservationId"] = new SelectList(_context.Reservations, "Id", "Id");
            return View();
        }

        // ===================== CREATE (POST) =====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReservationId,AdditionalCharges")] Billing billing)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Vehicle)
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(r => r.Id == billing.ReservationId);

            if (reservation == null)
            {
                ModelState.AddModelError("", "Reservation not found.");
            }

            if (ModelState.IsValid && reservation != null && reservation.Vehicle != null)
            {
                int totalDays = (reservation.EndDate - reservation.StartDate).Days;
                if (totalDays <= 0) totalDays = 1;

                billing.BaseAmount = reservation.Vehicle.PricePerDay * totalDays;
                billing.Tax = billing.BaseAmount * 0.05m;
                billing.TotalAmount = billing.BaseAmount + billing.Tax + billing.AdditionalCharges;
                billing.BillingDate = DateTime.Now;

                _context.Billings.Add(billing);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["ReservationId"] = new SelectList(_context.Reservations, "Id", "Id", billing.ReservationId);
            return View(billing);
        }

        // ===================== EDIT (GET) =====================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var billing = await _context.Billings.FindAsync(id);
            if (billing == null) return NotFound();

            ViewData["ReservationId"] = new SelectList(_context.Reservations, "Id", "Id", billing.ReservationId);
            return View(billing);
        }

        // ===================== EDIT (POST) =====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReservationId,AdditionalCharges")] Billing input)
        {
            if (id != input.Id) return NotFound();

            var existing = await _context.Billings.FindAsync(id);
            if (existing == null) return NotFound();

            var reservation = await _context.Reservations
                .Include(r => r.Vehicle)
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(r => r.Id == input.ReservationId);

            if (reservation == null)
            {
                ModelState.AddModelError("", "Reservation not found.");
            }

            if (ModelState.IsValid && reservation != null && reservation.Vehicle != null)
            {
                int totalDays = (reservation.EndDate - reservation.StartDate).Days;
                if (totalDays <= 0) totalDays = 1;

                existing.ReservationId = input.ReservationId;
                existing.AdditionalCharges = input.AdditionalCharges;
                existing.BaseAmount = reservation.Vehicle.PricePerDay * totalDays;
                existing.Tax = existing.BaseAmount * 0.05m;
                existing.TotalAmount = existing.BaseAmount + existing.Tax + existing.AdditionalCharges;
                existing.BillingDate = DateTime.Now;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["ReservationId"] = new SelectList(_context.Reservations, "Id", "Id", input.ReservationId);
            return View(input);
        }

        // ===================== DELETE (GET) =====================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var billing = await _context.Billings
                .Include(b => b.Reservation)
                    .ThenInclude(r => r!.Customer)
                .Include(b => b.Reservation)
                    .ThenInclude(r => r!.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (billing == null) return NotFound();

            return View(billing);
        }

        // ===================== DELETE (POST) =====================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var billing = await _context.Billings.FindAsync(id);
            if (billing != null)
            {
                _context.Billings.Remove(billing);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}