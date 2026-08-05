using IdentityManualApp.Models;

namespace PerfumesMVC.Models
{
    public class ReceitaProduto
    {
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }

        public int ReceitaId { get; set; }
        public Produto Receita { get; set; }

        public decimal Quantidade { get; set; }
        public string UnidadeMedida { get; set; }
    }
}
