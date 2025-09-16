using System.ComponentModel.DataAnnotations;

// Este modelo es la "receta secreta" de la cocina: solo la cocina (dominio y base de datos) la conoce y la entiende.
// No se la mostramos directamente a los clientes (usuarios de la API), porque podría tener ingredientes o detalles internos.
// Las reglas de validación más estrictas viven en los DTOs (el menú), pero aquí podemos poner reglas generales para la cocina.

namespace MiPrimeraAPI.domain.Entity;

// Representa una Tarea (Todo) en el dominio de la aplicación.
public class TodoItem
{
    // Identificador único de la tarea (clave primaria en la base).
    public long Id { get; set; }

    // El título es un ingrediente esencial, pero aquí solo ponemos reglas generales.
    // Las reglas de validación estrictas (como 'obligatorio' o 'máximo de caracteres') se aplican en el DTO, en la puerta del restaurante.
    [Required (ErrorMessage = "El Titulo de la tarea es obligatorio.")]
    [MaxLength (100, ErrorMessage = "El Titulo no puede exceder los 100 caracteres.")]
    public string? Title { get; set; } 

    // Indica si la tarea está completada.
    public bool IsComplete { get; set; }
}