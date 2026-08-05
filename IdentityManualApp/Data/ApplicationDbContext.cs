using IdentityManualApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PerfumesMVC.Models;

namespace IdentityManualApp.Data
{
    // Herdar de IdentityDbContext<ApplicationUser> já traz todas as tabelas
    // do Identity (AspNetUsers, AspNetRoles, etc.), sem precisar das Razor
    // Pages/Areas de Identity — o schema é criado via migrations normalmente.
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Fornecedor> Fornecedores { get; set; } = null!;
        public DbSet<Produto> Produtos { get; set; } = null!;
        public DbSet<Receita> Receitas { get; set; } = null!;
        public DbSet<ReceitaProduto> ReceitaProdutos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Produto>()
                .HasOne(p => p.Fornecedor)
                .WithMany(f => f.Produtos)
                .HasForeignKey(p => p.FornecedorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ReceitaProduto>()
                .HasKey(rp => new { rp.ReceitaId, rp.ProdutoId });

            builder.Entity<ReceitaProduto>(rp => { 
                rp.HasOne(rp => rp.Receita)
                .WithMany(r => r.ReceitaProdutos)
                .HasForeignKey(r => r.ReceitaId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<ReceitaProduto>(rp => {
                rp.HasOne(rp => rp.Produto)
                .WithMany(r => r.ReceitaProdutos)
                .HasForeignKey(r => r.ProdutoId)
                .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
