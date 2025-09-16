namespace MiPrimeraAPI.adapter.restful.v1.dto;

public class TodoItemDto
{
    public long Id { get; set; }
    public string? Title { get; set; }
    public bool IsComplete { get; set; }
}