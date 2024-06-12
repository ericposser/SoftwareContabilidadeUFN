using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SoftwareContabilidade.Models
{
    public class Venda
    {
        [Column("id")]
        [Display(Name = "id")]
        public int id { get; set; }

        [Column("mercadoria_id")]
        [Display(Name = "Mercadoria")]
        public virtual Mercadoria Mercadoria { get; set; }

        [Column("cliente_id")]
        [Display(Name = "Cliente")]
        public virtual Cliente Cliente { get; set; }

        [Column("quantidade")]
        [Display(Name = "Quantidade")]
        public int quantidade { get; set; }

        [DataType(DataType.Currency)]
        [Column("precoVenda")]
        [Display(Name = "Preço de Venda")]
        public float precoVenda { get; set; }
    }
}
