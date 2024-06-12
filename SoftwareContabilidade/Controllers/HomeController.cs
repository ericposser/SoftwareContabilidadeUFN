using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftwareContabilidade.Models;
using System.Diagnostics;

namespace SoftwareContabilidade.Controllers
{
    public class HomeController : Controller
    {
        private readonly Contexto _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, Contexto context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // Obtenha a contagem de fornecedores do banco de dados
            int quantidadeFornecedores = _context.Fornecedor.Count();
            int quantidadeMercadorias = _context.Mercadoria.Count();
            int quantidadeClientes = _context.Cliente.Count();
            int quantidadeCompras = _context.Compra.Count();
            int quantidadeVendas = _context.Venda.Count();

            // Obtenha as últimas compras do banco de dados
            var ultimasCompras = _context.Compra.Include(c => c.Fornecedor).Include(c => c.Mercadoria)
                                        .OrderByDescending(c => c.id)
                                        .Take(5)
                                        .ToList();

            // Obtenha as últimas vendas do banco de dados
            var ultimasVendas = _context.Venda.Include(v => v.Cliente).Include(v => v.Mercadoria)
                                        .OrderByDescending(v => v.id)
                                        .Take(5)
                                        .ToList();

            // Passe os dados para a view
            ViewBag.QuantidadeFornecedores = quantidadeFornecedores;
            ViewBag.QuantidadeMercadorias = quantidadeMercadorias;
            ViewBag.QuantidadeClientes = quantidadeClientes;
            ViewBag.QuantidadeCompras = quantidadeCompras;
            ViewBag.QuantidadeVendas = quantidadeVendas;
            ViewBag.UltimasCompras = ultimasCompras;
            ViewBag.UltimasVendas = ultimasVendas;

            return View();
        }




        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
