// Este DbContext es el "bibliotecario experto" del castillo.
// Su misión: traducir las intenciones de la cocina (servicio) a operaciones de base de datos.
// Cada DbSet<T> es como una “mesa” de libros (tabla). Aquí, TodoItems representa la tabla homónima.
// Mantenerlo separado permite cambiar de motor (SQLite, SQL Server, etc.) sin tocar la lógica de negocio.

using Microsoft.EntityFrameworkCore;
using MiPrimeraAPI.domain.Entity;

namespace MiPrimeraAPI.infrastructure;

public class TodoDbContext : DbContext
{
    // Por qué recibimos opciones por DI: así el bibliotecario sabe con qué “biblioteca” conectarse
    // (cadena de conexión, proveedor, etc.) configurado en Program.cs, sin acoplar detalles aquí.
    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
    {
    }

    // Esta "mesa" mapea a la tabla TodoItems. EF Core usa convenciones para columnas y claves.
    // Consultar/guardar sobre este DbSet hace que EF hable con la base según el proveedor configurado.
    public DbSet<TodoItem> TodoItems { get; set; }
}