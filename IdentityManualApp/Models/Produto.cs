using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityManualApp.Models
{
    public class Produto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do produto é obrigatório.")]
        [StringLength(150)]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "O preço é obrigatório.")]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0, 999999.99, ErrorMessage = "Informe um preço válido.")]
        [Display(Name = "Preço")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "A quantidade é obrigatória.")]
        [Range(0, int.MaxValue, ErrorMessage = "Informe uma quantidade válida.")]
        [Display(Name = "Quantidade em estoque")]
        public int Quantidade { get; set; }

        [Required(ErrorMessage = "Selecione um fornecedor.")]
        [Display(Name = "Fornecedor")]
        public int FornecedorId { get; set; }

        public Fornecedor? Fornecedor { get; set; }

        // Guarda o Id do usuário (Identity) que cadastrou o produto.
        public string? UsuarioId { get; set; }
    }
}
