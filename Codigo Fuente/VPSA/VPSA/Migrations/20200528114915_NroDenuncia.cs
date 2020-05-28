using Microsoft.EntityFrameworkCore.Migrations;

namespace VPSA.Migrations
{
    public partial class NroDenuncia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NroDenuncia",
                table: "Denuncias",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NroDenuncia",
                table: "Denuncias");
        }
    }
}
