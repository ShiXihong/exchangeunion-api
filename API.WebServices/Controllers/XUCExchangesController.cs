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
    public class XUCExchangesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly string _userName;
        private readonly string _category;

        public XUCExchangesController(ApplicationDbContext context,
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

        // GET: XUCExchanges
        public async Task<IActionResult> Index()
        {
            return View(await _context.XUCExchange.ToListAsync());
        }

        // GET: XUCExchanges/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var xUCExchange = await _context.XUCExchange
                .SingleOrDefaultAsync(m => m.Id == id);
            if (xUCExchange == null)
            {
                return NotFound();
            }

            return View(xUCExchange);
        }

        // GET: XUCExchanges/Create
        public IActionResult Create()
        {
            return View(new XUCExchange {
                CreateDate = DateTime.Now
            });
        }

        // POST: XUCExchanges/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Logo,XUC_BTC,XUC_BTC_Link,XUC_ETH,XUC_ETH_Link,XUC_USDT,XUC_USDT_Link, XUC_FIAT, XUC_FIAT_Link, Order, Status,CreateDate")] XUCExchange xUCExchange)
        {
            if (ModelState.IsValid)
            {
                xUCExchange.CreateDate = DateTime.Now;
                _context.Add(xUCExchange);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(xUCExchange);
        }

        // GET: XUCExchanges/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var xUCExchange = await _context.XUCExchange.SingleOrDefaultAsync(m => m.Id == id);
            if (xUCExchange == null)
            {
                return NotFound();
            }
            return View(xUCExchange);
        }

        // POST: XUCExchanges/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Logo,XUC_BTC,XUC_BTC_Link,XUC_ETH,XUC_ETH_Link,XUC_USDT,XUC_USDT_Link,XUC_FIAT, XUC_FIAT_Link, Order, Status,CreateDate")] XUCExchange xUCExchange)
        {
            if (id != xUCExchange.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(xUCExchange);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!XUCExchangeExists(xUCExchange.Id))
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
            return View(xUCExchange);
        }

        // GET: XUCExchanges/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var xUCExchange = await _context.XUCExchange
                .SingleOrDefaultAsync(m => m.Id == id);
            if (xUCExchange == null)
            {
                return NotFound();
            }

            return View(xUCExchange);
        }

        // POST: XUCExchanges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var xUCExchange = await _context.XUCExchange.SingleOrDefaultAsync(m => m.Id == id);
            _context.XUCExchange.Remove(xUCExchange);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool XUCExchangeExists(int id)
        {
            return _context.XUCExchange.Any(e => e.Id == id);
        }
    }
}
