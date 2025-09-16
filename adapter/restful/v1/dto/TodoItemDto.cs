namespace MiPrimeraAPI.adapter.restful.v1.dto;

// Este DTO es el "menú de salida": representa lo que el cliente ve cuando recibe una tarea desde la API.
// Incluye el Id, porque el cliente necesita saber cómo referirse a la tarea después de creada.
// Es diferente de la receta secreta (TodoItem) porque solo muestra lo que el cliente necesita saber.
public class TodoItemDto
{
    public long Id { get; set; }
    public string? Title { get; set; }
    public bool IsComplete { get; set; }
}