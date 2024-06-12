using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftwareContabilidade.Models
{
    [Table("Cliente")]
    public class Cliente
    {
        [Column("id")]
        [Display(Name = "id")]
        public int id { get; set; }

        [Column("nome")]
        [Display(Name = "Nome")]
        public string nome { get; set; }

        [Column("cidade")]
        [Display(Name = "Cidade")]
        public string cidade { get; set; }

        [Column("cnpj")]
        [Display(Name = "Cnpj")]
        public string? cnpj { get; set; }

        [Column("cpf")]
        [Display(Name = "Cpf")]
        public string? cpf { get; set; }
    }
}
