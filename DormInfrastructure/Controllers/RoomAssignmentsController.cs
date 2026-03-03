using DormDomain.Model;
using DormInfrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DormInfrastructure.Controllers
{
    public class RoomAssignmentsController : Controller
    {
        private readonly Do2Context _context;

        public RoomAssignmentsController(Do2Context context)
        {
            _context = context;
        }

        // GET: RoomAssignments
        public async Task<IActionResult> Index(int? id, string? name)
        {
           

            if (id == null) return RedirectToAction("Students", "Index");
            var student = await _context.Students.FindAsync(id);
            ViewBag.StudentId = id;
            ViewBag.StudentFullName = student?.FullName;
            var rByStudents = _context.RoomAssignments.Where(s => s.StudentId == id).Include(s => s.Student).Include(r => r.Room);


            return View(await rByStudents.ToListAsync());
        }

        // GET: RoomAssignments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomAssignment = await _context.RoomAssignments
                .Include(r => r.Room)
                .Include(r => r.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (roomAssignment == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Payments", new { id = roomAssignment.Id, name = roomAssignment.Room });
        }

        // GET: RoomAssignments/Create
        public IActionResult Create(int studentId)
        {
            var student = _context.Students.FirstOrDefault(s => s.Id == studentId);
            ViewBag.StudentId = studentId;
            ViewBag.StudentFullName = _context.Students.Where(a => a.Id == studentId).FirstOrDefault().FullName;
           
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "RoomNum");

            return View();
        }

        // POST: RoomAssignments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomId,StudentId,CheckInDate,CheckOutDate,Id")] RoomAssignment roomAssignment)
        {
            ModelState.Clear();
          

            if (ModelState.IsValid)
            {
               
                    _context.Add(roomAssignment);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new { id = roomAssignment.StudentId });
                
               
            }

         
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "RoomNum", roomAssignment.RoomId);
            ViewBag.StudentFullName = _context.Students.FirstOrDefault(s => s.Id == roomAssignment.StudentId)?.FullName;
            return View(roomAssignment);






        }

        // GET: RoomAssignments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomAssignment = await _context.RoomAssignments.FindAsync(id);
            if (roomAssignment == null)
            {
                return NotFound();
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "RoomNum", roomAssignment.RoomId);
            ViewBag.StudentFullName = roomAssignment.Student?.FullName;
            return View(roomAssignment);
        }

        // POST: RoomAssignments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoomId,StudentId,CheckInDate,CheckOutDate,Id")] RoomAssignment roomAssignment)
        {
            if (id != roomAssignment.Id)
            {
                return NotFound();
            }
            ModelState.Clear();
            ModelState.Remove("Room");
            ModelState.Remove("Student");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roomAssignment);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new { id = roomAssignment.StudentId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomAssignmentExists(roomAssignment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.StudentFullName = _context.Students.FirstOrDefault(s => s.Id == roomAssignment.StudentId)?.FullName;
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "RoomNum", roomAssignment.RoomId);
          
            return View(roomAssignment);
        }

        // GET: RoomAssignments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomAssignment = await _context.RoomAssignments
                .Include(r => r.Room)
                .Include(r => r.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (roomAssignment == null)
            {
                return NotFound();
            }

            return View(roomAssignment);
        }

        // POST: RoomAssignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var roomAssignment = await _context.RoomAssignments.FindAsync(id);
            int studentId = roomAssignment.StudentId;
            if (roomAssignment != null)
            {
                _context.RoomAssignments.Remove(roomAssignment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "RoomAssignments", new { id = studentId });
        }

        private bool RoomAssignmentExists(int id)
        {
            return _context.RoomAssignments.Any(e => e.Id == id);
        }
    }
}
