using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SoftwareContabilidade.Models
{
    [Table("Patrimonio")]
    public class Patrimonio
    {
        [Column("id")]
        [Display(Name = "id")]
        public int id { get; set; }

        [Column("nome")]
        [Display(Name = "Nome")]
        public string nome { get; set; }


        [Column("fornecedor_id")]
        [Display(Name = "Fornecedor")]
        public virtual Fornecedor Fornecedor { get; set; }

        [DataType(DataType.Currency)]
        [Column("preco")]
        [Display(Name = "Preço")]
        public float preco { get; set; }

    }
}
