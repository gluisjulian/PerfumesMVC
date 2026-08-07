using IdentityManualApp.Models;
using System.ComponentModel.DataAnnotations;

namespace PerfumesMVC.Models
{
    public class Receita
    {
        public int Id { get; set; }

        [Display(Name = "Titulo")]
        [StringLength(100)]
        public string Titulo { get; set; }

        [Display(Name = "Descrição Receita")]
        [StringLength(500)]
        public string Descricao { get; set; }

        public string UsuarioId { get; set; }

        public ICollection<ReceitaProduto> ReceitaProdutos { get; set; } = new List<ReceitaProduto>();

        public ICollection<Perfume> Perfumes { get; set; } = new List<Perfume>();
    }
}
