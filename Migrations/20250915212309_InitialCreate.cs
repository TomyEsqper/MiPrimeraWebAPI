// Las migraciones son los "planos de obra" del bibliotecario.
// Indican cómo construir o deshacer estanterías (tablas) en la base de datos a lo largo del tiempo.
// Up aplica los cambios (crear tabla), Down los revierte (eliminar tabla).

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiPrimeraAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Creamos la tabla TodoItems con sus columnas y clave primaria.
            migrationBuilder.CreateTable(
                name: "TodoItems",
                columns: table => new
                {
                    // Clave primaria autoincremental (el bibliotecario asigna el Id).
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),

                    // Título opcional (nullable) según el modelo actual.
                    Title = table.Column<string>(type: "TEXT", nullable: true),

                    // Bandera de completado (no nullable).
                    IsComplete = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    // Definición de la clave primaria.
                    table.PrimaryKey("PK_TodoItems", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revertimos la creación eliminando la tabla.
            migrationBuilder.DropTable(
                name: "TodoItems");
        }
    }
}
