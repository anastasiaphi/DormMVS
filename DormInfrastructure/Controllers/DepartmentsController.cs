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
    public class DepartmentsController : Controller
    {
        private readonly Do2Context _context;

        public DepartmentsController(Do2Context context)
        {
            _context = context;
        }

        // GET: Departments
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Faculties");
            var faculty = await _context.Faculties.FindAsync(id);
            if (faculty == null) return NotFound();
            ViewBag.FacultyId = id;
            ViewBag.FacultyName = faculty.Name;
            var departmentsByFaculty = _context.Departments.Where(f => f.Faculty.Id == id).Include(f => f.Faculty);

            return View(await departmentsByFaculty.ToListAsync());
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.Faculty)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Students", new { id = department.Id, name = department.Name });
        }
        

        // GET: Departments/Create
        public IActionResult Create(int facultyId)
        {
            ViewBag.FacultyId = facultyId;
            ViewBag.FacultyName = _context.Faculties.Where( a=> a.Id == facultyId).FirstOrDefault().Name;
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(  [Bind("FacultyId,Name,Id")] Department department)
        {
            Faculty faculty = _context.Faculties.FirstOrDefault(f => f.Id == department.FacultyId);
            department.Faculty = faculty;
            ModelState.Clear();
            TryValidateModel(department);
         
            if (ModelState.IsValid)
            {
                if (faculty != null)
                {
                    department.FacultyId = faculty.Id;
                }
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = department.FacultyId });
            }
            ViewData["FacultyId"] = new SelectList(_context.Faculties, "Id", "Name", department.FacultyId);
            return View(department);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
    .Include(d => d.Faculty) 
    .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }
            ViewData["FacultyId"] = new SelectList(_context.Faculties, "Id", "Name", department.FacultyId);
            ViewBag.DepartmentName = department.Name;
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FacultyId,Name,Id")] Department department)
        {
            if (id != department.Id)
            {
                return NotFound();
            }
            ModelState.Clear();
            ModelState.Remove("Faculty");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new { id = department.FacultyId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                
            }
            ViewData["FacultyId"] = new SelectList(_context.Faculties, "Id", "Address", department.FacultyId);
            return View(department);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.Faculty)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var department = await _context.Departments.FindAsync(id);
            int facultyId = department.FacultyId;
            if (department != null)
            {
                _context.Departments.Remove(department);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Departments", new { id = facultyId });
            // return RedirectToAction("Index", "Faculties", new { id = facultyId });
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }
    }
}
