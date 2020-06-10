using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VPSA.Migrations
{
    public partial class Db : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Empleados",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Dni = table.Column<int>(nullable: false),
                    Nombre = table.Column<string>(nullable: true),
                    Apellido = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EstadosDenuncia",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosDenuncia", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TiposDenuncia",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposDenuncia", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Denuncias",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Fecha = table.Column<DateTime>(nullable: true),
                    Calle = table.Column<string>(nullable: true),
                    Numero = table.Column<int>(nullable: false),
                    EntreCalles1 = table.Column<string>(nullable: true),
                    EntreCalles2 = table.Column<string>(nullable: true),
                    Descripcion = table.Column<string>(nullable: true),
                    TipoDenunciaId = table.Column<int>(nullable: false),
                    EstadoDenunciaId = table.Column<int>(nullable: true),
                    Legajo = table.Column<string>(nullable: true),
                    IpDenunciante = table.Column<string>(nullable: true),
                    NroDenuncia = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Denuncias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Denuncias_EstadosDenuncia_EstadoDenunciaId",
                        column: x => x.EstadoDenunciaId,
                        principalTable: "EstadosDenuncia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Denuncias_TiposDenuncia_TipoDenunciaId",
                        column: x => x.TipoDenunciaId,
                        principalTable: "TiposDenuncia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comentarios",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Descripcion = table.Column<string>(nullable: false),
                    DenunciaId = table.Column<int>(nullable: false),
                    EmpleadoId = table.Column<int>(nullable: false),
                    EstadoDenunciaId = table.Column<int>(nullable: false),
                    FechaCreacion = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comentarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comentarios_Denuncias_DenunciaId",
                        column: x => x.DenunciaId,
                        principalTable: "Denuncias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comentarios_Empleados_EmpleadoId",
                        column: x => x.EmpleadoId,
                        principalTable: "Empleados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comentarios_EstadosDenuncia_EstadoDenunciaId",
                        column: x => x.EstadoDenunciaId,
                        principalTable: "EstadosDenuncia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_DenunciaId",
                table: "Comentarios",
                column: "DenunciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_EmpleadoId",
                table: "Comentarios",
                column: "EmpleadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_EstadoDenunciaId",
                table: "Comentarios",
                column: "EstadoDenunciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Denuncias_EstadoDenunciaId",
                table: "Denuncias",
                column: "EstadoDenunciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Denuncias_TipoDenunciaId",
                table: "Denuncias",
                column: "TipoDenunciaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comentarios");

            migrationBuilder.DropTable(
                name: "Denuncias");

            migrationBuilder.DropTable(
                name: "Empleados");

            migrationBuilder.DropTable(
                name: "EstadosDenuncia");

            migrationBuilder.DropTable(
                name: "TiposDenuncia");
        }
    }
}
