using Microsoft.AspNetCore.Identity;

namespace IdentityManualApp.Models
{
    // Usuário customizado da aplicação, estendendo o IdentityUser padrão.
    public class ApplicationUser : IdentityUser
    {
        public string NomeCompleto { get; set; } = string.Empty;
    }
}
