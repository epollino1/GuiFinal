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
    public class CaloryLogController : Controller
    {
        private readonly FitnisContext _context;

        public CaloryLogController(FitnisContext context)
        {
            _context = context;
        }
        public IActionResult LoggedHome()
        {
            return View("LoggedHome");
        }
        // GET: CaloryLog
        public async Task<IActionResult> Index()
        {
            User user = _context.Users.FirstOrDefault(a => a.Email.Equals(User.Identity.Name));

            if (user is null)
            {
                return NotFound();
            }
            var fitnisContext = _context.CaloryLogs.Include(c => c.User);
            //fitnisContext = fitnisContext.Where(cl => cl.UserId.Equals(user.UserId));

            return View(await fitnisContext.ToListAsync());
        }


        // GET: CaloryLog/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.CaloryLogs == null)
            {
                return NotFound();
            }

            var caloryLog = await _context.CaloryLogs
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (caloryLog == null)
            {
                return NotFound();
            }

            return View(caloryLog);
        }

        // GET: CaloryLog/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: CaloryLog/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,LoggedAt,Title,Calories")] CaloryLog caloryLog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(caloryLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", caloryLog.UserId);
            return View(caloryLog);
        }

        // GET: CaloryLog/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.CaloryLogs == null)
            {
                return NotFound();
            }

            var caloryLog = await _context.CaloryLogs.FindAsync(id);
            if (caloryLog == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", caloryLog.UserId);
            return View(caloryLog);
        }

        // POST: CaloryLog/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,UserId,LoggedAt,Title,Calories")] CaloryLog caloryLog)
        {
            if (id != caloryLog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caloryLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaloryLogExists(caloryLog.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", caloryLog.UserId);
            return View(caloryLog);
        }

        // GET: CaloryLog/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.CaloryLogs == null)
            {
                return NotFound();
            }

            var caloryLog = await _context.CaloryLogs
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (caloryLog == null)
            {
                return NotFound();
            }

            return View(caloryLog);
        }

        // POST: CaloryLog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.CaloryLogs == null)
            {
                return Problem("Entity set 'FitnisContext.CaloryLogs'  is null.");
            }
            var caloryLog = await _context.CaloryLogs.FindAsync(id);
            if (caloryLog != null)
            {
                _context.CaloryLogs.Remove(caloryLog);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaloryLogExists(long id)
        {
          return (_context.CaloryLogs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
