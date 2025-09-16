using System.ComponentModel.DataAnnotations;

namespace MiPrimeraAPI.adapter.restful.v1.dto;

// Este DTO es el "menú de entrada": define exactamente qué puede pedir el cliente cuando quiere crear una tarea.
// No tiene Id porque el cliente no puede elegirlo; la cocina (dominio) lo asigna automáticamente.
// Aquí viven las reglas del "bouncer" (validación): solo dejamos pasar pedidos válidos.
public class CreateTodoDto
{
    [Required (ErrorMessage = "El titulo de la tarea es obligatorio.")]
    [MaxLength (100, ErrorMessage = "El titulo no puede exceder los 100 caracteres.")]
    public string? Title { get; set; }
    
    public bool IsComplete { get; set; }
}