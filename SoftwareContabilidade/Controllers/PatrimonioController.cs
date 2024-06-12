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
    public class PatrimonioController : Controller
    {
        private readonly Contexto _context;

        public PatrimonioController(Contexto context)
        {
            _context = context;
        }

        // GET: Patrimonio
        public async Task<IActionResult> Index(string fornecedorNome)
        {
            IQueryable<Patrimonio> patrimonios = _context.Patrimonio.Include(c => c.Fornecedor);

            if (!string.IsNullOrEmpty(fornecedorNome))
            {
                patrimonios = patrimonios.Where(c => c.Fornecedor.nome.Contains(fornecedorNome));
            }

            return View(patrimonios.ToList());
        }

        // GET: Patrimonio/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patrimonio = await _context.Patrimonio
                .FirstOrDefaultAsync(m => m.id == id);
            if (patrimonio == null)
            {
                return NotFound();
            }

            return View(patrimonio);
        }

        // GET: Patrimonio/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Fornecedores = new SelectList(await _context.Fornecedor.ToListAsync(), "id", "nome");
            return View();
        }

        // POST: Patrimonio/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int Fornecedor, [Bind("id,nome,preco")] Patrimonio patrimonio)
        {
            var fornecedor = await _context.Fornecedor.FindAsync(Fornecedor);                
           
            patrimonio.Fornecedor = fornecedor;

            _context.Add(patrimonio);
            await _context.SaveChangesAsync();

            TempData["Mensagem"] = "Patrimônio cadastrado com sucesso!";

            return RedirectToAction(nameof(Index));
        }

        // GET: Patrimonio/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Fornecedores = new SelectList(await _context.Fornecedor.ToListAsync(), "id", "nome");

            if (id == null)
            {
                return NotFound();
            }

            var patrimonio = await _context.Patrimonio.FindAsync(id);
            if (patrimonio == null)
            {
                return NotFound();
            }
            return View(patrimonio);
        }

        // POST: Patrimonio/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int Fornecedor, [Bind("id,nome,preco")] Patrimonio patrimonio)
        {
            if (id != patrimonio.id)
            {
                return NotFound();
            }

            var fornecedor = await _context.Fornecedor.FindAsync(Fornecedor);

            patrimonio.Fornecedor = fornecedor;


            try
                {
                    _context.Update(patrimonio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatrimonioExists(patrimonio.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            TempData["Mensagem"] = "Alteração feita com sucesso!";
            return RedirectToAction(nameof(Index));


        }

        // GET: Patrimonio/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patrimonio = await _context.Patrimonio
                .FirstOrDefaultAsync(m => m.id == id);
            if (patrimonio == null)
            {
                return NotFound();
            }

            return View(patrimonio);
        }

        // POST: Patrimonio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patrimonio = await _context.Patrimonio.FindAsync(id);
            if (patrimonio != null)
            {
                _context.Patrimonio.Remove(patrimonio);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatrimonioExists(int id)
        {
            return _context.Patrimonio.Any(e => e.id == id);
        }
    }
}
