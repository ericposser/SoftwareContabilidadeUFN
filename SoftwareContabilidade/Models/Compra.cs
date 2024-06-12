using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftwareContabilidade.Models
{
    [Table("Compra")]
    public class Compra
    {
        [Column("id")]
        [Display(Name = "id")]
        public int id { get; set; }

        [Column("mercadoria_id")]
        [Display(Name = "Mercadoria")]
        public virtual Mercadoria Mercadoria { get; set; }

        [Column("fornecedor_id")]
        [Display(Name = "Fornecedor")]
        public virtual Fornecedor Fornecedor { get; set; }


        [Column("quantidade")]
        [Display(Name = "Quantidade")]
        public int quantidade { get; set; }

        [DataType(DataType.Currency)]
        [Column("precoCusto")]
        [Display(Name = "Preço de Custo")]
        public float precoCusto { get; set; }

    }
}
