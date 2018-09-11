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
    public class XUCTeamsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly string _userName;
        private readonly string _category;

        public XUCTeamsController(ApplicationDbContext context,
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

        // GET: XUCTeams
        public async Task<IActionResult> Index()
        {
            return View(await _context.Team.ToListAsync());
        }

        // GET: XUCTeams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var xUCTeam = await _context.Team
                .SingleOrDefaultAsync(m => m.Id == id);
            if (xUCTeam == null)
            {
                return NotFound();
            }

            return View(xUCTeam);
        }

        // GET: XUCTeams/Create
        public IActionResult Create()
        {
            return View(new XUCTeam {
                CreateDate = DateTime.Now
            });
        }

        // POST: XUCTeams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ZHName,ENName,ZHPosition,ENPosition,ZHDescription,ENDescription,Photo,Type,Category,Order,Status,CreateDate")] XUCTeam xUCTeam)
        {
            if (ModelState.IsValid)
            {
                xUCTeam.Category = _category;
                xUCTeam.CreateDate = DateTime.Now;
                _context.Add(xUCTeam);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(xUCTeam);
        }

        // GET: XUCTeams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var xUCTeam = await _context.Team.SingleOrDefaultAsync(m => m.Id == id);
            if (xUCTeam == null)
            {
                return NotFound();
            }
            return View(xUCTeam);
        }

        // POST: XUCTeams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ZHName,ENName,ZHPosition,ENPosition,ZHDescription,ENDescription,Photo,Type,Category,Order,Status,CreateDate")] XUCTeam xUCTeam)
        {
            if (id != xUCTeam.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(xUCTeam);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!XUCTeamExists(xUCTeam.Id))
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
            return View(xUCTeam);
        }

        // GET: XUCTeams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var xUCTeam = await _context.Team
                .SingleOrDefaultAsync(m => m.Id == id);
            if (xUCTeam == null)
            {
                return NotFound();
            }

            return View(xUCTeam);
        }

        // POST: XUCTeams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var xUCTeam = await _context.Team.SingleOrDefaultAsync(m => m.Id == id);
            _context.Team.Remove(xUCTeam);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool XUCTeamExists(int id)
        {
            return _context.Team.Any(e => e.Id == id);
        }
    }
}
