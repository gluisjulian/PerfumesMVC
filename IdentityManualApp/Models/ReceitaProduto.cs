using IdentityManualApp.Models;
using System.ComponentModel.DataAnnotations;

namespace PerfumesMVC.Models
{
    public class ReceitaProduto
    {
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }

        public int ReceitaId { get; set; }
        public Receita Receita { get; set; }


        [Display(Name = "Quantidade Usada")]
        [Range(0, 999999.99, ErrorMessage = "Informe a quantidade.")]
        public decimal Quantidade { get; set; }


        [Display(Name = "Porcentagem")]
        [Range(0, 999999.99, ErrorMessage = "Informe a porcentagem.")]
        public decimal PorcentagemProduto { get; set; }


        [Display(Name = "Unidade Medida (g,ml,kg,L)")]
        [StringLength(2)]
        public string UnidadeMedida { get; set; }
    }
}
