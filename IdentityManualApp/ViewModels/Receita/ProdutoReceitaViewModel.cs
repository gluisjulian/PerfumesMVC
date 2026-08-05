namespace PerfumesMVC.ViewModels.Receita
{
    public class ProdutoReceitaViewModel
    {
        public int ProdutoId { get; set; }

        public string NomeProduto { get; set; } = string.Empty;

        public decimal Quantidade { get; set; }

        public decimal PorcentagemProduto { get; set; }

        public string UnidadeMedida { get; set; } = string.Empty;

        public bool Selecionado { get; set; }
    }
}
