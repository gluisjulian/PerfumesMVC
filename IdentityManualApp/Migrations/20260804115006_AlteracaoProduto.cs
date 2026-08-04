using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityManualApp.Migrations
{
    /// <inheritdoc />
    public partial class AlteracaoProduto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Quantidade",
                table: "Produtos",
                type: "decimal(10,3)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "UnidadeMedida",
                table: "Produtos",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnidadeMedida",
                table: "Produtos");

            migrationBuilder.AlterColumn<int>(
                name: "Quantidade",
                table: "Produtos",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,3)");
        }
    }
}
