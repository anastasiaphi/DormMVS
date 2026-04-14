using DormDomain.Model;
using DormInfrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DormInfrastructure.Controllers
{
    [Authorize(Roles = "admin, user")]
    public class TariffsController : Controller
    {
        private readonly Do2Context _context;

        public TariffsController(Do2Context context)
        {
            _context = context;
        }

        // GET: Tariffs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tariffs.ToListAsync());
        }

        // GET: Tariffs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tariff = await _context.Tariffs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tariff == null)
            {
                return NotFound();
            }

            //return View(tariff);
            return RedirectToAction("Index", "Rooms" , new {id = tariff.Id, name = tariff.TariffsName  });
        }
        [Authorize(Roles = "admin")]
        // GET: Tariffs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tariffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TariffsName,PricePerMonth,Id")] Tariff tariff)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tariff);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tariff);
        }
        [Authorize(Roles = "admin")]
        // GET: Tariffs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tariff = await _context.Tariffs.FindAsync(id);
            if (tariff == null)
            {
                return NotFound();
            }
            return View(tariff);
        }

        // POST: Tariffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TariffsName,PricePerMonth,Id")] Tariff tariff)
        {
            if (id != tariff.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tariff);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TariffExists(tariff.Id))
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
            return View(tariff);
        }
        [Authorize(Roles = "admin")]
        // GET: Tariffs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tariff = await _context.Tariffs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tariff == null)
            {
                return NotFound();
            }

            return View(tariff);
        }

        // POST: Tariffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tariff = await _context.Tariffs.FindAsync(id);
            if (tariff != null)
            {
                _context.Tariffs.Remove(tariff);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TariffExists(int id)
        {
            return _context.Tariffs.Any(e => e.Id == id);
        }
    }
}
