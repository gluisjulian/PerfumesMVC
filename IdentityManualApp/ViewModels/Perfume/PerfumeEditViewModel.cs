using System.ComponentModel.DataAnnotations;

namespace PerfumesMVC.ViewModels.Perfume
{
    public class PerfumeEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe o nome do perfume")]
        public string Nome { get; set; }

        public string Descricao { get; set; }

        [Display(Name = "Imagem do Perfume")]
        public IFormFile? ImagemArquivo { get; set; }

        // Só pra exibir o nome do arquivo atual na tela, não vem do form
        public string? ImagemAtualNome { get; set; }

    }
}
