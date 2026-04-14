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
    public class RoomsController : Controller
    {
        private readonly Do2Context _context;

        public RoomsController(Do2Context context)
        {
            _context = context;
        }

        // GET: Rooms
        public async Task<IActionResult> Index(int? id) 
        {
            if (id== null) return RedirectToAction("Tariffs", "Index");
            var tariff = await _context.Tariffs.FindAsync(id);
            ViewBag.TariffId = id;
            ViewBag.TariffName = tariff.TariffsName;
            var roomsByTariff = _context.Rooms.Where(r => r.TariffsId == id).Include(r => r.Tariffs);
          
            return View(await roomsByTariff.ToListAsync());
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(r => r.Tariffs)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Students", new { id = room.Id, name = room.RoomNum });
        }
        [Authorize(Roles = "admin")]
        // GET: Rooms/Create
        public IActionResult Create(int tariffId)
        {
            var tariff = _context.Tariffs.FirstOrDefault(s => s.Id == tariffId);
          
            ViewBag.TariffId = tariffId;
            ViewBag.TariffName = _context.Tariffs.Where(c => c.Id == tariffId).FirstOrDefault().TariffsName;

            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TariffsId,RoomNum,Capacity,Id")] Room room)
        {

            ModelState.Remove("Tariffs");

            if (ModelState.IsValid)
            {
                _context.Add(room);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index), new { id = room.TariffsId });



            }
            ViewData["TariffsId"] = new SelectList(_context.Tariffs, "Id", "TariffsName", room.TariffsId);
            return View(room);
        }
        [Authorize(Roles = "admin")]
        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            ViewData["TariffsId"] = new SelectList(_context.Tariffs, "Id", "TariffsName", room.TariffsId);
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TariffsId,RoomNum,Capacity,Id")] Room room)
        {
            if (id != room.Id)
            {
                return NotFound();
            }
            ModelState.Clear();
            ModelState.Remove("Tariff");
            

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new { id = room.TariffsId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.Id))
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
            ViewData["TariffsId"] = new SelectList(_context.Tariffs, "Id", "TariffsName", room.TariffsId);
            return View(room);
        }
        [Authorize(Roles = "admin")]
        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(r => r.Tariffs)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            int tariffId = room.TariffsId;
            if (room != null)
            {
                _context.Rooms.Remove(room);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Rooms", new { id = tariffId });
        }

        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.Id == id);
        }
    }
}
