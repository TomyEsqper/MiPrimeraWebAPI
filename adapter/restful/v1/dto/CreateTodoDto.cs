using System.ComponentModel.DataAnnotations;

namespace MiPrimeraAPI.adapter.restful.v1.dto;

public class CreateTodoDto
{
    [Required (ErrorMessage = "El titulo de la tarea es obligatorio.")]
    [MaxLength (100, ErrorMessage = "El titulo no puede exceder los 100 caracteres.")]
    public string? Title { get; set; }
    
    public bool IsComplete { get; set; }
    
}