using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SoftwareContabilidade.Models;

namespace SoftwareContabilidade.Controllers
{
    public class CompraController : Controller
    {
        private readonly Contexto _context;

        public CompraController(Contexto context)
        {
            _context = context;
        }

        // GET: Venda
        public IActionResult Index(string fornecedorNome, string mercadoriaNome)
        {
            IQueryable<Compra> compras = _context.Compra.Include(c => c.Fornecedor).Include(c => c.Mercadoria);

            if (!string.IsNullOrEmpty(fornecedorNome))
            {
                compras = compras.Where(c => c.Fornecedor.nome.Contains(fornecedorNome));
            }

            if (!string.IsNullOrEmpty(mercadoriaNome))
            {
                compras = compras.Where(c => c.Mercadoria.nome.Contains(mercadoriaNome));
            }

            return View(compras.ToList());
        }


        // GET: Venda/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Mercadorias = new SelectList(await _context.Mercadoria.ToListAsync(), "id", "nome");
            ViewBag.Fornecedores = new SelectList(await _context.Fornecedor.ToListAsync(), "id", "nome");
            return View();
        }

        // POST: Compra/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int Fornecedor, int Mercadoria, [Bind("id,quantidade,precoCusto")] Compra compra)
        {
            // Buscar a mercadoria e o cliente selecionados no banco de dados
            var fornecedor = await _context.Fornecedor.FindAsync(Fornecedor);
            var mercadoria = await _context.Mercadoria.FindAsync(Mercadoria);

            // Associar a mercadoria e o cliente à venda
            compra.Mercadoria = mercadoria;
            compra.Fornecedor = fornecedor;


            // Adicionar a venda ao contexto e salvar as alterações
            _context.Add(compra);
            await _context.SaveChangesAsync();

            TempData["Mensagem"] = "Compra cadastrada com sucesso!";

            return RedirectToAction(nameof(Index));
        }

        // GET: Compra/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Mercadorias = new SelectList(await _context.Mercadoria.ToListAsync(), "id", "nome");
            ViewBag.Fornecedores = new SelectList(await _context.Fornecedor.ToListAsync(), "id", "nome");

            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compra.FindAsync(id);

            if (compra == null)
            {
                return NotFound();
            }
            return View(compra);
        }

        // POST: Compra/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int Mercadoria, int Fornecedor, [Bind("id,quantidade,precoCusto")] Compra compra)
        {
            if (id != compra.id)
            {
                return NotFound();
            }

            // Buscar a mercadoria e o cliente selecionados no banco de dados
            var mercadoria = await _context.Mercadoria.FindAsync(Mercadoria);
            var fornecedor = await _context.Fornecedor.FindAsync(Fornecedor);

            // Associar a mercadoria e o cliente à venda
            compra.Mercadoria = mercadoria;
            compra.Fornecedor = fornecedor;

                try
                {
                    _context.Update(compra);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompraExists(compra.id))
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

        // GET: Compra/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compra
                .FirstOrDefaultAsync(m => m.id == id);
            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }

        // POST: Compra/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var compra = await _context.Compra.FindAsync(id);
            if (compra != null)
            {
                _context.Compra.Remove(compra);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompraExists(int id)
        {
            return _context.Compra.Any(e => e.id == id);
        }

        public IActionResult ExportarPDFCompra()
        {
            // Carregue as compras incluindo as entidades relacionadas
            var compras = _context.Compra.Include(c => c.Fornecedor).Include(c => c.Mercadoria).ToList();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                PdfWriter.GetInstance(pdfDoc, memoryStream);

                pdfDoc.Open();

                // Adicione o título ao documento PDF
                Paragraph title = new Paragraph("Relatório de Compras", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16f));
                title.Alignment = Element.ALIGN_CENTER;
                pdfDoc.Add(title);
                pdfDoc.Add(Chunk.NEWLINE);

                // Crie uma tabela para os dados das compras (agora com 4 colunas)
                PdfPTable table = new PdfPTable(4);
                table.WidthPercentage = 100; // Defina a largura da tabela como 100% da largura da página

                // Adicione as classes do Bootstrap à tabela
                table.AddCell(new PdfPCell(new Phrase("Fornecedor", FontFactory.GetFont(FontFactory.HELVETICA_BOLD))) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Mercadoria", FontFactory.GetFont(FontFactory.HELVETICA_BOLD))) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Quantidade", FontFactory.GetFont(FontFactory.HELVETICA_BOLD))) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Preço de Custo", FontFactory.GetFont(FontFactory.HELVETICA_BOLD))) { BackgroundColor = BaseColor.LIGHT_GRAY });

                // Adicione os dados das compras à tabela
                foreach (var compra in compras)
                {
                    // Se houver um fornecedor associado à compra, use o nome do fornecedor, caso contrário, use "N/A"
                    string fornecedorNome = compra.Fornecedor != null ? compra.Fornecedor.nome : "N/A";

                    // Se houver uma mercadoria associada à compra, use o nome da mercadoria, caso contrário, use "N/A"
                    string mercadoriaNome = compra.Mercadoria != null ? compra.Mercadoria.nome : "N/A";

                    table.AddCell(new PdfPCell(new Phrase(fornecedorNome)));
                    table.AddCell(new PdfPCell(new Phrase(mercadoriaNome)));
                    table.AddCell(new PdfPCell(new Phrase(compra.quantidade.ToString())));
                    table.AddCell(new PdfPCell(new Phrase(compra.precoCusto.ToString())));
                }

                // Adicione a tabela ao documento PDF
                pdfDoc.Add(table);

                pdfDoc.Close();

                return File(memoryStream.ToArray(), "application/pdf", "RelatorioCompras.pdf");
            }
        }

        public IActionResult Search(string fornecedorNome, string mercadoriaNome)
        {
            IQueryable<Compra> compras = _context.Compra.Include(c => c.Fornecedor).Include(c => c.Mercadoria);

            if (!string.IsNullOrEmpty(fornecedorNome))
            {
                compras = compras.Where(c => c.Fornecedor.nome.Contains(fornecedorNome));
            }

            if (!string.IsNullOrEmpty(mercadoriaNome))
            {
                compras = compras.Where(c => c.Mercadoria.nome.Contains(mercadoriaNome));
            }

            // Retorna para a página Index, mantendo os parâmetros de busca
            return RedirectToAction("Index", new { fornecedorNome, mercadoriaNome });
        }
    }
}
