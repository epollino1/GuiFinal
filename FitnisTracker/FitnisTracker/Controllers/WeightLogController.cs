using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitnisTracker.Models;

namespace FitnisTracker.Controllers
{
    public class WeightLogController : Controller
    {
        private readonly FitnisContext _context;

        public WeightLogController(FitnisContext context)
        {
            _context = context;
        }

        // GET: WeightLog
        public async Task<IActionResult> Index()
        {
            var fitnisContext = _context.WeightLogs.Include(w => w.User);
            return View(await fitnisContext.ToListAsync());
        }

        // GET: WeightLog/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.WeightLogs == null)
            {
                return NotFound();
            }

            var weightLog = await _context.WeightLogs
                .Include(w => w.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (weightLog == null)
            {
                return NotFound();
            }

            return View(weightLog);
        }

        // GET: WeightLog/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: WeightLog/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,LoggedAt,CurrentWeight")] WeightLog weightLog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(weightLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", weightLog.UserId);
            return View(weightLog);
        }

        // GET: WeightLog/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.WeightLogs == null)
            {
                return NotFound();
            }

            var weightLog = await _context.WeightLogs.FindAsync(id);
            if (weightLog == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", weightLog.UserId);
            return View(weightLog);
        }

        // POST: WeightLog/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,UserId,LoggedAt,CurrentWeight")] WeightLog weightLog)
        {
            if (id != weightLog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(weightLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WeightLogExists(weightLog.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", weightLog.UserId);
            return View(weightLog);
        }

        // GET: WeightLog/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.WeightLogs == null)
            {
                return NotFound();
            }

            var weightLog = await _context.WeightLogs
                .Include(w => w.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (weightLog == null)
            {
                return NotFound();
            }

            return View(weightLog);
        }

        // POST: WeightLog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.WeightLogs == null)
            {
                return Problem("Entity set 'FitnisContext.WeightLogs'  is null.");
            }
            var weightLog = await _context.WeightLogs.FindAsync(id);
            if (weightLog != null)
            {
                _context.WeightLogs.Remove(weightLog);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WeightLogExists(long id)
        {
          return (_context.WeightLogs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
