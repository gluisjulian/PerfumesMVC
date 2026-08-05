namespace PerfumesMVC.ViewModels.Receita
{
    public class ReceitaCreateViewModel
    {
        public string Titulo { get; set; } = string.Empty;

        public string? Descricao { get; set; }


        public List<ProdutoReceitaViewModel> Produtos { get; set; }
            = new List<ProdutoReceitaViewModel>();
    }
}
