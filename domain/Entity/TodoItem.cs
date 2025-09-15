// Namespace que ubica y agrupa el modelo de dominio.
namespace MiPrimeraAPI.domain.Entity;

// Representa una Tarea (Todo) en el dominio de la aplicación.
public class TodoItem
{
    // Identificador único de la tarea.
    public long Id { get; set; }

    // Título o descripción breve; opcional (puede ser null).
    public string? Title { get; set; } 

    // Indica si la tarea está completada.
    public bool IsComplete { get; set; }
}