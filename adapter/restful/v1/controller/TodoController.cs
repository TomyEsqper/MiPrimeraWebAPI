using Microsoft.AspNetCore.Mvc;
using MiPrimeraAPI.application.Service;
using MiPrimeraAPI.domain.Entity;
using MiPrimeraAPI.adapter.restful.v1.dto;

namespace MiPrimeraAPI.adapter.restful.v1.controller;

// Este controlador es el "mesero" profesional del restaurante.
// Ahora, gracias a Validación y DTOs, el mesero puede:
// 1. Dejar pasar solo pedidos válidos (el "bouncer" revisa los DTOs con reglas de validación).
// 2. Recibir y entregar "menús" (DTOs) en vez de recetas secretas (entidades de dominio).
// 3. Realizar conversiones manuales y explícitas entre menús (DTOs) y recetas (entidades).
[ApiController]
[Route("api/v1/[controller]")]
public class TodoController : ControllerBase
{
    // El mesero ahora tiene acceso al menú (servicio de tareas).
    private readonly ITodoService _todoService;

    // El constructor recibe el menú (servicio).
    public TodoController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    // --- FLUJO PROFESIONAL DE UNA PETICIÓN HTTP ---
    // 1. El cliente hace una petición (por ejemplo, GET, POST, etc.).
    // 2. El "bouncer" (validación automática) revisa el DTO de entrada (si aplica).
    // 3. El mesero (controller) traduce explícitamente entre DTOs y entidades cuando corresponde.
    // 4. El mesero pasa la receta a la cocina (servicio de dominio).
    // 5. La cocina responde con una receta secreta, que el mesero traduce de vuelta a un DTO para el cliente.

    // --- ACCIÓN: Obtener todas las tareas ---
    // GET api/v1/Todo
    // El mesero pide todas las recetas a la cocina, las traduce a menús y las entrega al cliente.
    [HttpGet]
    public IActionResult GetAll()
    {
        var todoItems = _todoService.GetAllTodos();

        var todoDtos = new List<TodoItemDto>(todoItems.Count);
        foreach (var item in todoItems)
        {
            todoDtos.Add(ToDto(item));
        }

        return Ok(todoDtos);
    }

    // --- ACCIÓN: Obtener una tarea por id ---
    // GET api/v1/Todo/{id}
    // El mesero busca la receta por id, la traduce a menú y la entrega al cliente.
    [HttpGet("{id}")]
    public IActionResult GetById(long id)
    {
        var todoItem = _todoService.GetTodoById(id);
        if (todoItem == null)
        {
            return NotFound();
        }
        return Ok(ToDto(todoItem));
    }

    // --- ACCIÓN: Crear una nueva tarea ---
    // POST api/v1/Todo
    // El cliente entrega un pedido usando el menú de entrada (CreateTodoDto).
    // El bouncer revisa que el pedido sea válido (validación automática).
    // El mesero traduce el pedido a receta secreta (TodoItem), lo pasa a la cocina, y luego traduce la respuesta a menú de salida (TodoItemDto).
    [HttpPost]
    public IActionResult Create([FromBody] CreateTodoDto createDto)
    {
        var todoItem = FromCreateDto(createDto);
        var createdTodo = _todoService.CreateTodo(todoItem);
        var responseDto = ToDto(createdTodo);
        return CreatedAtAction(nameof(GetById), new { id = responseDto.Id }, responseDto);
    }

    // --- ACCIÓN: Actualizar una tarea existente ---
    // PUT api/v1/Todo/{id}
    // El cliente entrega un menú de entrada (CreateTodoDto).
    // El bouncer revisa la validez.
    // El mesero busca la receta original y aplica los cambios manualmente, y la cocina actualiza la base.
    [HttpPut("{id}")]
    public IActionResult Update(long id, [FromBody] CreateTodoDto updatedDto)
    {
        var existingTodo = _todoService.GetTodoById(id);
        if (existingTodo == null)
        {
            return NotFound();
        }

        UpdateEntityFromDto(updatedDto, existingTodo);
        _todoService.UpdateTodo(id, existingTodo);

        return NoContent();
    }

    // --- ACCIÓN: Eliminar una tarea ---
    // DELETE api/v1/Todo/{id}
    // El mesero verifica que la receta exista y le pide a la cocina que la elimine.
    [HttpDelete("{id}")]
    public IActionResult Delete(long id)
    {
        var existingTodo = _todoService.GetTodoById(id);
        if (existingTodo == null)
        {
            return NotFound();
        }
        _todoService.DeleteTodo(id);
        return NoContent();
    }

    // --- Mapeos manuales entre DTOs y Entidades ---
    private static TodoItemDto ToDto(TodoItem item)
    {
        return new TodoItemDto
        {
            Id = item.Id,
            Title = item.Title,
            IsComplete = item.IsComplete
        };
    }

    private static TodoItem FromCreateDto(CreateTodoDto dto)
    {
        return new TodoItem
        {
            Title = dto.Title,
            IsComplete = dto.IsComplete
        };
    }

    private static void UpdateEntityFromDto(CreateTodoDto dto, TodoItem entity)
    {
        entity.Title = dto.Title;
        entity.IsComplete = dto.IsComplete;
    }
}
