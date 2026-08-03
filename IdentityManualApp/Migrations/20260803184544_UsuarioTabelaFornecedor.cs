using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityManualApp.Migrations
{
    /// <inheritdoc />
    public partial class UsuarioTabelaFornecedor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "Fornecedores",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Fornecedores");
        }
    }
}
