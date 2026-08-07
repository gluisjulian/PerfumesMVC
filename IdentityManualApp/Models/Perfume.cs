using IdentityManualApp.Models;
using System.ComponentModel.DataAnnotations;
using static PerfumesMVC.Models.Enums;

namespace PerfumesMVC.Models
{
    public class Perfume
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }

        // Campos para a imagem
        public byte[]? ImagemDados { get; set; }
        public string? ImagemTipo { get; set; }   // ex: "image/png", "image/jpeg"
        public string? ImagemNome { get; set; }   // nome original do arquivo

        public string UsuarioId { get; set; }
        public ApplicationUser Usuario { get; set; }


        //RELACIONAMENTO RECEITA
        public int ReceitaId { get; set; }
        public Receita Receita { get; set; }
    }
}
