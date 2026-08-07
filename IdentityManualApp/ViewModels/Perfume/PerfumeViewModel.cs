using System.ComponentModel.DataAnnotations;

namespace PerfumesMVC.ViewModels.Perfume
{
    public class PerfumeViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }

        [Display(Name = "Receita")]
        public int ReceitaId { get; set; }

        [Display(Name = "Imagem do Perfume")]
        public IFormFile? ImagemArquivo { get; set; }
    }
}
