// Este modelo es el "plano" o el "ADN" de una Tarea dentro del dominio.
// Define la forma de los datos que viajan entre capas: controlador (mesero), servicio (cocina) y DbContext (bibliotecario).
// Mantenerlo simple y neutral al transporte (HTTP/JSON) facilita su reutilización y pruebas.

namespace MiPrimeraAPI.domain.Entity;

// Representa una Tarea (Todo) en el dominio de la aplicación.
public class TodoItem
{
    // Identificador único de la tarea (clave primaria en la base).
    public long Id { get; set; }

    // Título o descripción breve; opcional (puede ser null).
    // Nota: las reglas de validación de entrada viven en el DTO (borde HTTP), no aquí.
    public string? Title { get; set; } 

    // Indica si la tarea está completada.
    public bool IsComplete { get; set; }
}