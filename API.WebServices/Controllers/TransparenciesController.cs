using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using API.WebServices.Data;
using API.WebServices.Models;
using Microsoft.AspNetCore.Authorization;

namespace API.WebServices.Controllers
{
    [Authorize]
    public class TransparenciesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransparenciesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Transparencies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Transparency.ToListAsync());
        }

        // GET: Transparencies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transparency = await _context.Transparency
                .SingleOrDefaultAsync(m => m.Id == id);
            if (transparency == null)
            {
                return NotFound();
            }

            return View(transparency);
        }

        // GET: Transparencies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Transparencies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Address,Time,Amount,Reason,TxHash,Active,CreateDate")] Transparency transparency)
        {
            if (ModelState.IsValid)
            {
                transparency.CreateDate = DateTime.Now;
                _context.Add(transparency);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(transparency);
        }

        // GET: Transparencies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transparency = await _context.Transparency.SingleOrDefaultAsync(m => m.Id == id);
            if (transparency == null)
            {
                return NotFound();
            }
            return View(transparency);
        }

        // POST: Transparencies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Address,Time,Amount,Reason,TxHash,Active,CreateDate")] Transparency transparency)
        {
            if (id != transparency.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transparency);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransparencyExists(transparency.Id))
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
            return View(transparency);
        }

        // GET: Transparencies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transparency = await _context.Transparency
                .SingleOrDefaultAsync(m => m.Id == id);
            if (transparency == null)
            {
                return NotFound();
            }

            return View(transparency);
        }

        // POST: Transparencies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transparency = await _context.Transparency.SingleOrDefaultAsync(m => m.Id == id);
            _context.Transparency.Remove(transparency);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransparencyExists(int id)
        {
            return _context.Transparency.Any(e => e.Id == id);
        }
    }
}
