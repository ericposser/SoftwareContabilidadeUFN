using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftwareContabilidade.Models
{
    [Table("Relatorio")]
    public class Relatorio
    {
        [Column("id")]
        [Display(Name = "id")]
        public int id { get; set; }
    }
}
