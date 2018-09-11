using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using API.WebServices.Data;
using API.WebServices.Models;
using Microsoft.AspNetCore.Identity;
using API.WebServices.Utils;
using Microsoft.AspNetCore.Authorization;

namespace API.WebServices.Controllers
{
    [Authorize]
    public class PortfoliosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly string _userName;
        private readonly string _category;

        public PortfoliosController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManage)
        {
            _userManager = userManager;
            _signInManager = signInManage;
            _context = context;
            _userName = _userManager.GetUserName(_signInManager.Context.User);
            if (_userName == "admin@exchangeunion.com")
            {
                _category = EnumUtils.Category.ExchangeUnion.ToString();
            }
            else if (_userName == "admin@digitalfinancegroup.net")
            {
                _category = EnumUtils.Category.DigitalFinanceGroup.ToString();
            }
        }

        // GET: Portfolios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Portfolio.ToListAsync());
        }

        // GET: Portfolios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portfolio = await _context.Portfolio
                .SingleOrDefaultAsync(m => m.Id == id);
            if (portfolio == null)
            {
                return NotFound();
            }

            return View(portfolio);
        }

        // GET: Portfolios/Create
        public IActionResult Create()
        {
            return View(new Portfolio {
                CreateDate = DateTime.Now
            });
        }

        // POST: Portfolios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Logo,Link,Description,Category,ShowHome,Status,CreateDate")] Portfolio portfolio)
        {
            if (ModelState.IsValid)
            {
                portfolio.CreateDate = DateTime.Now;
                _context.Add(portfolio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(portfolio);
        }

        // GET: Portfolios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portfolio = await _context.Portfolio.SingleOrDefaultAsync(m => m.Id == id);
            if (portfolio == null)
            {
                return NotFound();
            }
            return View(portfolio);
        }

        // POST: Portfolios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Logo,Link,Description,Category,ShowHome,Status,CreateDate")] Portfolio portfolio)
        {
            if (id != portfolio.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(portfolio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PortfolioExists(portfolio.Id))
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
            return View(portfolio);
        }

        // GET: Portfolios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portfolio = await _context.Portfolio
                .SingleOrDefaultAsync(m => m.Id == id);
            if (portfolio == null)
            {
                return NotFound();
            }

            return View(portfolio);
        }

        // POST: Portfolios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var portfolio = await _context.Portfolio.SingleOrDefaultAsync(m => m.Id == id);
            _context.Portfolio.Remove(portfolio);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PortfolioExists(int id)
        {
            return _context.Portfolio.Any(e => e.Id == id);
        }
    }
}
