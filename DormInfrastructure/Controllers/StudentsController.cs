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
    public class StudentsController : Controller
    {
        private readonly Do2Context _context;

        public StudentsController(Do2Context context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null) return RedirectToAction(" Departments", "Index");
            var department = await _context.Departments.FindAsync(id);
            ViewBag.DepartmentId = id;
            ViewBag.DepartmentName = department.Name;
            var studentByDepartment = _context.Students.Where(s => s.Department.Id == id).Include(s => s.Department).Include(s => s.Degree);

            return View(await studentByDepartment.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Degree)
                .Include(s => s.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "RoomAssignments", new { id = student.Id, name = student.PhoneNumber });
        }

        // GET: Students/Create
        public IActionResult Create(int departmentId)
        {
            ViewBag.DepartmentId = departmentId;
            
            ViewBag.DepartmentName = _context.Departments.Where(n => n.Id == departmentId).FirstOrDefault().Name;
          
            ViewData["DegreeId"] = new SelectList(_context.Degrees, "Id", "DegreeName");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( [Bind("DepartmentId,DegreeId,FirstName,MiddleName,LastName,PhoneNumber,Id")] Student student)
        {
          
            Department department = _context.Departments.FirstOrDefault(f => f.Id == student.DepartmentId);
            student.Department = department;
            ModelState.Clear();
            TryValidateModel(student);

            if (ModelState.IsValid)
            {
                if (department != null)
                { 
                    student.DepartmentId = department.Id;
                }
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = student.DepartmentId });
            }
            ViewData["DegreeId"] = new SelectList(_context.Degrees, "Id", "DegreeName", student.DegreeId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", student.DepartmentId);
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["DegreeId"] = new SelectList(_context.Degrees, "Id", "DegreeName", student.DegreeId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", student.DepartmentId);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DepartmentId,DegreeId,FirstName,MiddleName,LastName,PhoneNumber,Id")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }
            ModelState.Clear();
            ModelState.Remove("Department");
            ModelState.Remove("Degree");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new { id = student.DepartmentId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
              
            }
            ViewData["DegreeId"] = new SelectList(_context.Degrees, "Id", "DegreeName", student.DegreeId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", student.DepartmentId);
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Degree)
                .Include(s => s.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            int departmentId = student.DepartmentId;
            if (student != null)
            {
                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Students", new { id = departmentId });

        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
