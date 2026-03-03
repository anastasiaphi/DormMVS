using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DormDomain.Model;
using DormInfrastructure;

namespace DormInfrastructure.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly Do2Context _context;

        public PaymentsController(Do2Context context)
        {
            _context = context;
        }

        // GET: Payments
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) return RedirectToAction(" Faculties", "Index");
          

           
            ViewBag.RoomAssignmentId = id;
            ViewBag.RoomAssignmentName = name;

            var paymentsByRA = _context.Payments.Where(f => f.RoomAssignment.Id == id).Include(f => f.RoomAssignment).Include(p => p.PaymentsStatus);

            return View(await paymentsByRA.ToListAsync());
        }

        // GET: Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.PaymentsStatus)
                .Include(p => p.RoomAssignment)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // GET: Payments/Create
        public IActionResult Create(int roomAssignmentId)
        {
           



            var assignment = _context.RoomAssignments.Include(a => a.Student).FirstOrDefault(a => a.Id == roomAssignmentId);

            ViewBag.RoomAssignmentId = roomAssignmentId;

          
            ViewData["PaymentsStatusId"] = new SelectList(_context.PaymentsStatuses, "Id", "StatusName");
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int roomAssignmentId, [Bind("RoomAssignmentId,PaymentsStatusId,Amount,PaymentDate,Id")] Payment payment)
        {
            payment.RoomAssignmentId = roomAssignmentId;
            ModelState.Clear();
            if (ModelState.IsValid)
            {
                _context.Add(payment);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index), new { id = payment.RoomAssignmentId });

                
            }
              ViewData["PaymentsStatusId"] = new SelectList(_context.PaymentsStatuses, "Id", "StatusName", payment.PaymentsStatusId);
            return View(payment);
        }

        // GET: Payments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            ViewData["PaymentsStatusId"] = new SelectList(_context.PaymentsStatuses, "Id", "StatusName", payment.PaymentsStatusId);
            ViewData["RoomAssignmentId"] = new SelectList(_context.RoomAssignments, "Id", "Id", payment.RoomAssignmentId);
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoomAssignmentId,PaymentsStatusId,Amount,PaymentDate,Id")] Payment payment)
        {
            if (id != payment.Id)
            {
                return NotFound();
            }
            ModelState.Clear();
            ModelState.Remove("RoomAssignmentId");
            ModelState.Remove("PaymentsStatusId");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new { id = payment.RoomAssignmentId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
              
            }
            ViewData["PaymentsStatusId"] = new SelectList(_context.PaymentsStatuses, "Id", "StatusName", payment.PaymentsStatusId);
        
            return View(payment);
        }

        // GET: Payments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.PaymentsStatus)
                .Include(p => p.RoomAssignment)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            int roomAssignmentId = payment.RoomAssignmentId;
            if (payment != null)
            {
                _context.Payments.Remove(payment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Payments", new { id = roomAssignmentId });


        

        }

        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.Id == id);
        }
    }
}
