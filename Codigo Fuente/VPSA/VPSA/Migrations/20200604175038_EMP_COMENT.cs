using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VPSA.Migrations
{
    public partial class EMP_COMENT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DenunciaViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Fecha = table.Column<DateTime>(nullable: true),
                    Calle = table.Column<string>(nullable: true),
                    Numero = table.Column<int>(nullable: true),
                    EntreCalles1 = table.Column<string>(nullable: true),
                    EntreCalles2 = table.Column<string>(nullable: true),
                    Descripcion = table.Column<string>(nullable: false),
                    TipoDenunciaId = table.Column<int>(nullable: false),
                    EstadoDenunciaId = table.Column<int>(nullable: true),
                    Legajo = table.Column<string>(nullable: true),
                    IpDenunciante = table.Column<string>(nullable: true),
                    NroDenuncia = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DenunciaViewModel", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DenunciaViewModel");
        }
    }
}
