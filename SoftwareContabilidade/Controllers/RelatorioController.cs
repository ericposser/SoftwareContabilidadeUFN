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
    public class RelatorioController : Controller
    {
        private readonly Contexto _context;

        public RelatorioController(Contexto context)
        {
            _context = context;
        }

        // GET: Relatorio
        public async Task<IActionResult> Index()
        {
            // Obter todos os patrimônios
            var patrimonios = await _context.Patrimonio.ToListAsync();
            var mercadorias = await _context.Mercadoria.ToListAsync();
            var icms = await _context.Icsms.ToListAsync();         

            // Calcular o valor total dos patrimônios
            var totalPreco = patrimonios.Sum(p => p.preco);
            var totalPrecoM = mercadorias.Sum(p => p.qtdEstoque);
            var totalIcmsRecuperar = icms.Where(i => i.tipo == "Recuperar").Sum(i => i.valor);
            var totalIcmsRecolher = icms.Where(i => i.tipo == "Recolher").Sum(i => i.valor);
            var totalPrecoVenda = mercadorias.Sum(p => p.precoVenda);

            // Passar o valor total para a visão
            ViewData["TotalPreco"] = totalPreco;
            ViewData["TotalPrecoM"] = totalPrecoM;
            ViewData["TotalIcmsRecuperar"] = totalIcmsRecuperar;
            ViewData["TotalIcmsRecolher"] = totalIcmsRecolher;
            ViewData["totalPrecoVenda"] = totalPrecoVenda;

            return View();
        }




        // GET: Relatorio/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var relatorio = await _context.Relatorio
                .FirstOrDefaultAsync(m => m.id == id);
            if (relatorio == null)
            {
                return NotFound();
            }

            return View(relatorio);
        }

        // GET: Relatorio/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Relatorio/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id")] Relatorio relatorio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(relatorio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(relatorio);
        }

        // GET: Relatorio/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var relatorio = await _context.Relatorio.FindAsync(id);
            if (relatorio == null)
            {
                return NotFound();
            }
            return View(relatorio);
        }

        // POST: Relatorio/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id")] Relatorio relatorio)
        {
            if (id != relatorio.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(relatorio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RelatorioExists(relatorio.id))
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
            return View(relatorio);
        }

        // GET: Relatorio/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var relatorio = await _context.Relatorio
                .FirstOrDefaultAsync(m => m.id == id);
            if (relatorio == null)
            {
                return NotFound();
            }

            return View(relatorio);
        }

        // POST: Relatorio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var relatorio = await _context.Relatorio.FindAsync(id);
            if (relatorio != null)
            {
                _context.Relatorio.Remove(relatorio);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RelatorioExists(int id)
        {
            return _context.Relatorio.Any(e => e.id == id);
        }
    }
}
