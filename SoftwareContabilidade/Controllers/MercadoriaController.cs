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
    public class MercadoriaController : Controller
    {
        private readonly Contexto _context;

        public MercadoriaController(Contexto context)
        {
            _context = context;
        }

        // GET: Mercadoria
        public IActionResult Index(string filtro)
        {
            IQueryable<Mercadoria> mercadorias = _context.Mercadoria;

            if (!string.IsNullOrEmpty(filtro))
            {
                // Verifica se o filtro é um número
                int codigo;
                bool isCodigo = int.TryParse(filtro, out codigo);

                // Aplica o filtro
                if (isCodigo)
                {
                    mercadorias = mercadorias.Where(m => m.codigo == codigo);
                }
                else
                {
                    mercadorias = mercadorias.Where(m => m.nome.Contains(filtro));
                }
            }

            return View(mercadorias.ToList());
        }

        // GET: Mercadoria/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mercadoria = await _context.Mercadoria
                .FirstOrDefaultAsync(m => m.id == id);
            if (mercadoria == null)
            {
                return NotFound();
            }

            return View(mercadoria);
        }

        // GET: Mercadoria/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Mercadoria/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,codigo,nome,qtdEstoque,precoVenda,precoCusto")] Mercadoria mercadoria)
        {
            if (ModelState.IsValid)
            {
                // Calcular 17% do precoCusto
                double icmsValor = mercadoria.precoCusto * 0.17; // 17% do precoCusto

                // Criar uma instância de Icsm
                Icsm icms = new Icsm
                {
                    valor = (float)icmsValor,
                    tipo = "Recuperar"
                };

                // Adicionar a instância de Icsm ao contexto do banco de dados IcsmContext
                _context.Add(icms);

                // Adicionar a mercadoria ao contexto do banco de dados
                _context.Add(mercadoria);

                // Salvar as alterações nos dois contextos de banco de dados
                await _context.SaveChangesAsync();

                TempData["Mensagem"] = "Mercadoria cadastrada com sucesso!";

                return RedirectToAction(nameof(Index));
            }
            return View(mercadoria);
        }

        // GET: Mercadoria/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mercadoria = await _context.Mercadoria.FindAsync(id);
            if (mercadoria == null)
            {
                return NotFound();
            }
            return View(mercadoria);
        }

        // POST: Mercadoria/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,codigo,nome,qtdEstoque,precoVenda,precoCusto")] Mercadoria mercadoria)
        {
            if (id != mercadoria.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mercadoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MercadoriaExists(mercadoria.id))
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
            return View(mercadoria);
        }

        // GET: Mercadoria/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mercadoria = await _context.Mercadoria
                .FirstOrDefaultAsync(m => m.id == id);
            if (mercadoria == null)
            {
                return NotFound();
            }

            return View(mercadoria);
        }

        // POST: Mercadoria/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mercadoria = await _context.Mercadoria.FindAsync(id);
            if (mercadoria != null)
            {
                _context.Mercadoria.Remove(mercadoria);
            }

            await _context.SaveChangesAsync();
            TempData["Mensagem"] = "Excluído com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private bool MercadoriaExists(int id)
        {
            return _context.Mercadoria.Any(e => e.id == id);
        }

        public IActionResult Search(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return RedirectToAction("Index");
            }

            int codigo;
            bool isCodigo = int.TryParse(searchString, out codigo);

            var mercadorias = _context.Mercadoria.Where(m =>
                m.codigo == codigo ||
                m.nome.Contains(searchString)
            ).ToList();

            // Passa o filtro para a view Index
            return RedirectToAction("Index", new { filtro = searchString });
        }
    }
}
