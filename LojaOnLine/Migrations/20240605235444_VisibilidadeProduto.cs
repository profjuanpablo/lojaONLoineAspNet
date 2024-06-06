using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LojaOnLine.Migrations
{
    /// <inheritdoc />
    public partial class VisibilidadeProduto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "VisivelNaPaginaInicial",
                table: "Produtos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VisivelNaPaginaInicial",
                table: "Produtos");
        }
    }
}
