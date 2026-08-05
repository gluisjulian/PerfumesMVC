using System.ComponentModel.DataAnnotations;

namespace PerfumesMVC.ViewModels.Receita
{
    public class ReceitaEditViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Titulo { get; set; } = string.Empty;

        public string? Descricao { get; set; }

        public List<ReceitaProdutoViewModel> Produtos { get; set; } = new();
    }
}
