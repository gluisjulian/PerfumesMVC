using IdentityManualApp.Models;

namespace PerfumesMVC.Models
{
    public class Receita
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }

        public string UsuarioId { get; set; }

        public ICollection<ReceitaProduto> ReceitaProdutos { get; set; } = new List<ReceitaProduto>();
    }
}
