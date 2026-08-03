using System.ComponentModel.DataAnnotations;

namespace IdentityManualApp.Models
{
    public class Fornecedor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do fornecedor é obrigatório.")]
        [StringLength(150)]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CNPJ é obrigatório.")]
        [StringLength(18)]
        [Display(Name = "CNPJ")]
        public string Cnpj { get; set; } = string.Empty;

        [StringLength(20)]
        [Display(Name = "Telefone")]
        public string? Telefone { get; set; }

        [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
        [Display(Name = "E-mail")]
        public string? Email { get; set; }

        [StringLength(250)]
        [Display(Name = "Endereço")]
        public string? Endereco { get; set; }

        public string? UsuarioId { get; set; }

        // Navegação: um fornecedor pode ter vários produtos.
        public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
    }
}
