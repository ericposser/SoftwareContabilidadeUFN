using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace SoftwareContabilidade.Models
{
    [Table("Mercadoria")]
    public class Mercadoria
    {
        [Column("id")]
        [Display(Name = "id")]
        public int id { get; set; }

        [Column("codigo")]
        [Display(Name = "Código")]
        public int codigo { get; set; }

        [Column("nome")]
        [Display(Name = "Nome")]
        public string nome { get; set; }

        [Column("qtdEstoque")]
        [Display(Name = "Quantidade em Estoque")]
        public int qtdEstoque { get; set; }

        [DataType(DataType.Currency)]
        [Column("precoVenda")]
        [Display(Name = "Preço de Venda")]
        public float precoVenda { get; set; }

        [DataType(DataType.Currency)]
        [Column("precoCusto")]
        [Display(Name = "Preço de Custo")]
        public float precoCusto { get; set; }

    }
}
