using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftwareContabilidade.Models
{
    [Table("Fornecedor")]
    public class Fornecedor
    {
        [Column("id")]
        [Display(Name = "id")]
        public int id { get; set; }

        [Column("nome")]
        [Display(Name = "Nome")]
        public string nome { get; set; }

        [Column("cnpj")]
        [Display(Name = "Cnpj")]
        public string cnpj { get; set; }

        [Column("cidade")]
        [Display(Name = "Cidade")]
        public string cidade { get; set; }
    }
}
