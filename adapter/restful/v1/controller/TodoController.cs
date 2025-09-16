using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MiPrimeraAPI.application.Service;
using MiPrimeraAPI.domain.Entity;
using MiPrimeraAPI.adapter.restful.v1.dto;

namespace MiPrimeraAPI.adapter.restful.v1.controller;

// Este controlador es el "mesero" profesional del restaurante.
// Ahora, gracias a Validación, DTOs y AutoMapper, el mesero puede:
// 1. Dejar pasar solo pedidos válidos (el "bouncer" revisa los DTOs con reglas de validación).
// 2. Recibir y entregar "menús" (DTOs) en vez de recetas secretas (entidades de dominio).
// 3. Usar el "traductor universal" (AutoMapper) para convertir entre menús y recetas sin esfuerzo manual.
[ApiController]
[Route("api/v1/[controller]")]
public class TodoController : ControllerBase
{
    // El mesero ahora tiene acceso tanto al menú (servicio de tareas) como al traductor universal (AutoMapper).
    private readonly ITodoService _todoService;
    private readonly IMapper _mapper;

    // El constructor recibe el menú (servicio) y el traductor universal (AutoMapper).
    // Así, el mesero puede traducir cualquier pedido del cliente a la receta secreta y viceversa, de forma profesional y automática.
    public TodoController(ITodoService todoService, IMapper mapper)
    {
        _todoService = todoService;
        _mapper = mapper;
    }

    // --- FLUJO PROFESIONAL DE UNA PETICIÓN HTTP ---
    // 1. El cliente hace una petición (por ejemplo, GET, POST, etc.).
    // 2. El "bouncer" (validación automática) revisa el DTO de entrada (si aplica).
    // 3. El mesero (controller) recibe el DTO y, si es necesario, lo traduce a la receta secreta (entidad de dominio) usando AutoMapper.
    // 4. El mesero pasa la receta a la cocina (servicio de dominio).
    // 5. La cocina responde con una receta secreta, que el mesero traduce de vuelta a un DTO para el cliente.

    // --- ACCIÓN: Obtener todas las tareas ---
    // GET api/v1/Todo
    // El mesero pide todas las recetas a la cocina, las traduce a menús y las entrega al cliente.
    [HttpGet]
    public IActionResult GetAll()
    {
        var todoItems = _todoService.GetAllTodos();
        // Traducción automática: de recetas secretas (TodoItem) a menús públicos (TodoItemDto).
        var todoDtos = _mapper.Map<List<TodoItemDto>>(todoItems);
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
        var todoDto = _mapper.Map<TodoItemDto>(todoItem);
        return Ok(todoDto);
    }

    // --- ACCIÓN: Crear una nueva tarea ---
    // POST api/v1/Todo
    // El cliente entrega un pedido usando el menú de entrada (CreateTodoDto).
    // El bouncer revisa que el pedido sea válido (validación automática).
    // El mesero traduce el pedido a receta secreta (TodoItem), lo pasa a la cocina, y luego traduce la respuesta a menú de salida (TodoItemDto).
    [HttpPost]
    public IActionResult Create([FromBody] CreateTodoDto createDto)
    {
        // Traducción automática: del menú de entrada (CreateTodoDto) a receta secreta (TodoItem).
        var todoItem = _mapper.Map<TodoItem>(createDto);
        var createdTodo = _todoService.CreateTodo(todoItem);
        // Traducción automática: de receta secreta a menú de salida (TodoItemDto).
        var responseDto = _mapper.Map<TodoItemDto>(createdTodo);
        return CreatedAtAction(nameof(GetById), new { id = responseDto.Id }, responseDto);
    }

    // --- ACCIÓN: Actualizar una tarea existente ---
    // PUT api/v1/Todo/{id}
    // El cliente entrega un menú de entrada (TodoItemDto o CreateTodoDto).
    // El bouncer revisa la validez.
    // El mesero busca la receta original, aplica los cambios usando el traductor universal, y la cocina actualiza la base.
    [HttpPut("{id}")]
    public IActionResult Update(long id, [FromBody] CreateTodoDto updatedDto)
    {
        var existingTodo = _todoService.GetTodoById(id);
        if (existingTodo == null)
        {
            return NotFound();
        }
        // Traducción automática: aplica los cambios del menú de entrada a la receta secreta existente.
        _mapper.Map(updatedDto, existingTodo);
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
}
