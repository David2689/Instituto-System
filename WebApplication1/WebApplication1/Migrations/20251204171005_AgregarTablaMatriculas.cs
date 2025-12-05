using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTablaMatriculas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Matriculas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NombreCompleto = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Sexo = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    InformacionMedica = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    NombreResponsable = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Telefono = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Turno = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Especialidad = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    CertificadoRuta = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matriculas", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Matriculas");
        }
    }
}
