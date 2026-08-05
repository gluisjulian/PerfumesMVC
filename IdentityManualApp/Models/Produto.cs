using PerfumesMVC.Models;
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
        [Column(TypeName = "decimal(10,3)")]
        [Range(0, 999999.999, ErrorMessage = "Informe a quantidade.")]
        public decimal Quantidade { get; set; }

        [StringLength(2)]
        [Required(ErrorMessage = "A Unidade de medida é obrigatório.")]
        [Display(Name = "Unidade de Medida")]
        public string UnidadeMedida { get; set; }

        [Required(ErrorMessage = "Selecione um fornecedor.")]
        [Display(Name = "Fornecedor")]
        public int FornecedorId { get; set; }

        public Fornecedor? Fornecedor { get; set; }

        [Display(Name = "Lote")]
        [Required(ErrorMessage = "O Lote é obrigatório.")]
        [StringLength(50)]
        public string Lote { get; set; }


        [Display(Name = "Data Fabricação")]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataFabricacao { get; set; }

        [Display(Name = "Data Validade")]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataValidade { get; set; }


        // Guarda o Id do usuário (Identity) que cadastrou o produto.
        public string? UsuarioId { get; set; }

        public ICollection<ReceitaProduto> ReceitaProdutos { get; set; } = new List<ReceitaProduto>();
    }
}
