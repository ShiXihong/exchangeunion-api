using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.WebServices.Data;
using API.WebServices.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.WebServices.Controllers
{
    [Authorize]
    public class UpdataTranslationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _environment;

        public UpdataTranslationController(ApplicationDbContext context,
            IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] List<Translation> translation)
        {
            if (translation.Count <= 0)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.UpdateRange(translation);
                    await _context.SaveChangesAsync();
                    return Ok(true);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            return View(translation);
        }
    }
}