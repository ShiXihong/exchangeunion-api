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
    public class DFGTeamsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly string _userName;
        private readonly string _category;

        public DFGTeamsController(ApplicationDbContext context,
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

        // GET: DFGTeams
        public async Task<IActionResult> Index()
        {
            return View(await _context.DFGTeam.ToListAsync());
        }

        // GET: DFGTeams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dFGTeam = await _context.DFGTeam
                .SingleOrDefaultAsync(m => m.Id == id);
            if (dFGTeam == null)
            {
                return NotFound();
            }

            return View(dFGTeam);
        }

        // GET: DFGTeams/Create
        public IActionResult Create()
        {
            return View(new DFGTeam {
                CreateDate = DateTime.Now
            });
        }

        // POST: DFGTeams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ZHName,ENName,ZHPosition,ENPosition,ZHDescription,ENDescription,Photo,Email,LinkedIn,Twitter,Type,Category,Order,Status,CreateDate")] DFGTeam dFGTeam)
        {
            if (ModelState.IsValid)
            {
                dFGTeam.CreateDate = DateTime.Now;
                dFGTeam.Category = _category;
                _context.Add(dFGTeam);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dFGTeam);
        }

        // GET: DFGTeams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dFGTeam = await _context.DFGTeam.SingleOrDefaultAsync(m => m.Id == id);
            if (dFGTeam == null)
            {
                return NotFound();
            }
            return View(dFGTeam);
        }

        // POST: DFGTeams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ZHName,ENName,ZHPosition,ENPosition,ZHDescription,ENDescription,Photo,Email,LinkedIn,Twitter,Type,Order,Category,Status,CreateDate")] DFGTeam dFGTeam)
        {
            if (id != dFGTeam.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dFGTeam);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DFGTeamExists(dFGTeam.Id))
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
            return View(dFGTeam);
        }

        // GET: DFGTeams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dFGTeam = await _context.DFGTeam
                .SingleOrDefaultAsync(m => m.Id == id);
            if (dFGTeam == null)
            {
                return NotFound();
            }

            return View(dFGTeam);
        }

        // POST: DFGTeams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dFGTeam = await _context.DFGTeam.SingleOrDefaultAsync(m => m.Id == id);
            _context.DFGTeam.Remove(dFGTeam);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DFGTeamExists(int id)
        {
            return _context.DFGTeam.Any(e => e.Id == id);
        }
    }
}
