using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SoftwareContabilidade.Models;

namespace SoftwareContabilidade.Controllers
{
    public class IcmsController : Controller
    {
        private readonly Contexto _context;

        public IcmsController(Contexto context)
        {
            _context = context;
        }

        // GET: Icms
        public async Task<IActionResult> Index()
        {
            return View(await _context.Icsms.ToListAsync());
        }

        // GET: Icms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var icsm = await _context.Icsms
                .FirstOrDefaultAsync(m => m.id == id);
            if (icsm == null)
            {
                return NotFound();
            }

            return View(icsm);
        }

        // GET: Icms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Icms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,valor,tipo")] Icsm icsm)
        {
            if (ModelState.IsValid)
            {
                _context.Add(icsm);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(icsm);
        }

        // GET: Icms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var icsm = await _context.Icsms.FindAsync(id);
            if (icsm == null)
            {
                return NotFound();
            }
            return View(icsm);
        }

        // POST: Icms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,valor,tipo")] Icsm icsm)
        {
            if (id != icsm.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(icsm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IcsmExists(icsm.id))
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
            return View(icsm);
        }

        // GET: Icms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var icsm = await _context.Icsms
                .FirstOrDefaultAsync(m => m.id == id);
            if (icsm == null)
            {
                return NotFound();
            }

            return View(icsm);
        }

        // POST: Icms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var icsm = await _context.Icsms.FindAsync(id);
            if (icsm != null)
            {
                _context.Icsms.Remove(icsm);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IcsmExists(int id)
        {
            return _context.Icsms.Any(e => e.id == id);
        }
    }
}
