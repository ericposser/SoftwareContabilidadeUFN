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
    public class VendaController : Controller
    {
        private readonly Contexto _context;

        public VendaController(Contexto context)
        {
            _context = context;
        }

        // GET: Venda
        public IActionResult Index(string clienteNome, string mercadoriaNome)
        {
            IQueryable<Venda> vendas = _context.Venda.Include(v => v.Cliente).Include(v => v.Mercadoria);

            if (!string.IsNullOrEmpty(clienteNome))
            {
                vendas = vendas.Where(v => v.Cliente.nome.Contains(clienteNome));
            }

            if (!string.IsNullOrEmpty(mercadoriaNome))
            {
                vendas = vendas.Where(v => v.Mercadoria.nome.Contains(mercadoriaNome));
            }

            return View(vendas.ToList());
        }


        // GET: Venda/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Mercadorias = new SelectList(await _context.Mercadoria.ToListAsync(), "id", "nome");
            ViewBag.Clientes = new SelectList(await _context.Cliente.ToListAsync(), "id", "nome");
            return View();
        }


        // POST: Venda/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int Mercadoria, int Cliente, [Bind("id,quantidade,precoVenda")] Venda venda)
        {
            // Buscar a mercadoria e o cliente selecionados no banco de dados
            var mercadoria = await _context.Mercadoria.FindAsync(Mercadoria);
            var cliente = await _context.Cliente.FindAsync(Cliente);

            // Associar a mercadoria e o cliente à venda
            venda.Mercadoria = mercadoria;
            venda.Cliente = cliente;

            double icmsValor = venda.precoVenda * 0.17;

            Icsm icms = new Icsm
            {
                valor = (float)icmsValor,
                tipo = "Recolher"
            };

            // Adicionar a venda ao contexto e salvar as alterações
            _context.Add(venda);
            _context.Add(icms);
            await _context.SaveChangesAsync();

            TempData["Mensagem"] = "Venda cadastrada com sucesso!";

            return RedirectToAction(nameof(Index));
         
        }


        // GET: Venda/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Mercadorias = new SelectList(await _context.Mercadoria.ToListAsync(), "id", "nome");
            ViewBag.Clientes = new SelectList(await _context.Cliente.ToListAsync(), "id", "nome");
            
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Venda.FindAsync(id);

            if (venda == null)
            {
                return NotFound();
            }
            return View();
        }

        // POST: Venda/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int Mercadoria, int Cliente, [Bind("id,quantidade,precoVenda")] Venda venda)
        {
            if (id != venda.id)
            {
                return NotFound();
            }

            // Buscar a mercadoria e o cliente selecionados no banco de dados
            var mercadoria = await _context.Mercadoria.FindAsync(Mercadoria);
            var cliente = await _context.Cliente.FindAsync(Cliente);

            // Associar a mercadoria e o cliente à venda
            venda.Mercadoria = mercadoria;
            venda.Cliente = cliente;
            
                try
                {
                    _context.Update(venda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendaExists(venda.id))
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

        // GET: Venda/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Venda
                .FirstOrDefaultAsync(m => m.id == id);
            if (venda == null)
            {
                return NotFound();
            }

            return View(venda);
        }

        // POST: Venda/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venda = await _context.Venda.FindAsync(id);
            if (venda != null)
            {
                _context.Venda.Remove(venda);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VendaExists(int id)
        {
            return _context.Venda.Any(e => e.id == id);
        }


        public IActionResult ExportarPDF()
        {
            // Carregue as vendas incluindo as entidades relacionadas
            var vendas = _context.Venda.Include(v => v.Cliente).Include(v => v.Mercadoria).ToList();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                PdfWriter.GetInstance(pdfDoc, memoryStream);

                pdfDoc.Open();

                // Adicione o título ao documento PDF
                Paragraph title = new Paragraph("Relatório de Vendas", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16f));
                title.Alignment = Element.ALIGN_CENTER;
                pdfDoc.Add(title);
                pdfDoc.Add(Chunk.NEWLINE);

                // Crie uma tabela para os dados das vendas (agora com 4 colunas)
                PdfPTable table = new PdfPTable(4);
                table.WidthPercentage = 100; // Defina a largura da tabela como 100% da largura da página

                // Adicione as classes do Bootstrap à tabela
                table.AddCell(new PdfPCell(new Phrase("Cliente", FontFactory.GetFont(FontFactory.HELVETICA_BOLD))) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Mercadoria", FontFactory.GetFont(FontFactory.HELVETICA_BOLD))) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Quantidade", FontFactory.GetFont(FontFactory.HELVETICA_BOLD))) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Preço de Venda", FontFactory.GetFont(FontFactory.HELVETICA_BOLD))) { BackgroundColor = BaseColor.LIGHT_GRAY });

                // Adicione os dados das vendas à tabela
                foreach (var venda in vendas)
                {
                    // Se houver um cliente associado à venda, use o nome do cliente, caso contrário, use "N/A"
                    string clienteNome = venda.Cliente != null ? venda.Cliente.nome : "N/A";

                    // Se houver uma mercadoria associada à venda, use o nome da mercadoria, caso contrário, use "N/A"
                    string mercadoriaNome = venda.Mercadoria != null ? venda.Mercadoria.nome : "N/A";

                    table.AddCell(new PdfPCell(new Phrase(clienteNome)));
                    table.AddCell(new PdfPCell(new Phrase(mercadoriaNome)));
                    table.AddCell(new PdfPCell(new Phrase(venda.quantidade.ToString())));
                    table.AddCell(new PdfPCell(new Phrase(venda.precoVenda.ToString())));
                }

                // Adicione a tabela ao documento PDF
                pdfDoc.Add(table);

                pdfDoc.Close();

                return File(memoryStream.ToArray(), "application/pdf", "RelatorioVendas.pdf");
            }
        }

        public IActionResult Search(string clienteNome, string mercadoriaNome)
        {
            IQueryable<Venda> vendas = _context.Venda.Include(v => v.Cliente).Include(v => v.Mercadoria);

            if (!string.IsNullOrEmpty(clienteNome))
            {
                vendas = vendas.Where(v => v.Cliente.nome.Contains(clienteNome));
            }

            if (!string.IsNullOrEmpty(mercadoriaNome))
            {
                vendas = vendas.Where(v => v.Mercadoria.nome.Contains(mercadoriaNome));
            }

            // Retorna para a página Index, mantendo os parâmetros de busca
            return RedirectToAction("Index", new { clienteNome, mercadoriaNome });
        }

    }
}
