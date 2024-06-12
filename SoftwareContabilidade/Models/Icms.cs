using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftwareContabilidade.Models
{
    [Table("Icms")]
    public class Icsm
    {
        [Column("id")]
        [Display(Name = "id")]
        public int id { get; set; }

        [DataType(DataType.Currency)]
        [Column("valor")]
        [Display(Name = "Valor")]
        public float valor { get; set; }

        [Column("tipo")]
        [Display(Name = "Imposto")]
        public string tipo { get; set; }
    }
}
