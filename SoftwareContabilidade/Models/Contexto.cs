using Microsoft.EntityFrameworkCore;

namespace SoftwareContabilidade.Models
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options){ }

        public DbSet<Mercadoria> Mercadoria { get; set; }
        public DbSet<Fornecedor> Fornecedor { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Compra> Compra { get; set; }
        public DbSet<Venda> Venda { get; set; }
        public DbSet<Icsm> Icsms { get; set; }
        public DbSet<Patrimonio> Patrimonio { get; set; }
        public DbSet<Relatorio> Relatorio { get; set; }
    }
}
