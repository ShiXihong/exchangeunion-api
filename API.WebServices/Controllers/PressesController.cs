using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using API.WebServices.Data;
using API.WebServices.Models;
using API.WebServices.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace API.WebServices.Controllers
{
    [Authorize]
    public class PressesController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly string _userName;
        private readonly string _category;

        public PressesController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _userName = _userManager.GetUserName(_signInManager.Context.User);
            if (_userName.IndexOf("@exchangeunion.com") != -1)
            {
                _category = EnumUtils.Category.ExchangeUnion.ToString();
            }
            else if (_userName == "admin@digitalfinancegroup.net")
            {
                _category = EnumUtils.Category.DigitalFinanceGroup.ToString();
            }
            else if (_userName == "admin@etclabs.org")
            {
                _category = EnumUtils.Category.ETCLabs.ToString();
            }
        }

        // GET: Presses
        public async Task<IActionResult> Index()
        {            
            return View(await _context.Press.Where(p => p.Category == _category).ToListAsync());
        }

        // GET: Presses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var press = await _context.Press
                .SingleOrDefaultAsync(m => m.Id == id);
            if (press == null)
            {
                return NotFound();
            }

            return View(press);
        }

        // GET: Presses/Create
        public IActionResult Create()
        {
            return View(new Press {
                Language = EnumUtils.Language.All.ToString(),
                Date = DateTime.Now,
                CreateDate = DateTime.Now
            });
        }

        // POST: Presses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ImageUrl,Link,Description,Date,Source,Status,Category,CreateDate, Language")] Press press)
        {
            if (ModelState.IsValid)
            {
                press.Category = _category;
                press.CreateDate = DateTime.Now;
                _context.Add(press);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(press);
        }

        // GET: Presses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var press = await _context.Press.SingleOrDefaultAsync(m => m.Id == id);
            if (press == null)
            {
                return NotFound();
            }
            return View(press);
        }

        // POST: Presses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ImageUrl,Link,Description,Date,Source,Status, CreateDate, Language")] Press press)
        {
            if (id != press.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    press.Category = _category;
                    _context.Update(press);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PressExists(press.Id))
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
            return View(press);
        }

        // GET: Presses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var press = await _context.Press
                .SingleOrDefaultAsync(m => m.Id == id);
            if (press == null)
            {
                return NotFound();
            }

            return View(press);
        }

        // POST: Presses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var press = await _context.Press.SingleOrDefaultAsync(m => m.Id == id);
            _context.Press.Remove(press);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PressExists(int id)
        {
            return _context.Press.Any(e => e.Id == id);
        }
    }
}
