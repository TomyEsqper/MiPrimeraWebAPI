using Microsoft.AspNetCore.Mvc; // Importa atributos y tipos base para construir controladores y manejar HTTP.
// Nota: el controlador es el "mesero" del restaurante; es la ÚNICA capa que habla con el mundo exterior (HTTP).
using MiPrimeraAPI.application.Service; // El controlador conoce solamente el CONTRATO (interfaz) del servicio: el "menú".
using MiPrimeraAPI.domain.Entity;       // El tipo de datos que viaja en las respuestas (y que entiende el servicio/la cocina).
using MiPrimeraAPI.adapter.restful.v1.dto; // DTOs de entrada/salida propios de la capa de transporte (puertas del castillo).

namespace MiPrimeraAPI.adapter.restful.v1.controller; // Organiza las "puertas" (endpoints) bajo un espacio lógico.

// [ApiController] activa validación automática del modelo y respuestas 400 si fallan,
// además de ayudar con el binding de parámetros. Es como tener un "bouncer" que revisa la entrada.
[ApiController]
// Define la ruta base para este controlador.
// [controller] se reemplaza por "Todo" (nombre de la clase sin "Controller").
// Esto crea la puerta del castillo por donde entran las peticiones REST de Todo.
[Route("api/v1/[controller]")]
public class TodoController : ControllerBase // Hereda utilidades para construir respuestas HTTP (Ok, NotFound, etc.).
{
    // Dependencia principal del controlador: el contrato del servicio de tareas (el "menú").
    // readonly: se asigna en el constructor y no cambia su referencia -> fortaleza del acoplamiento débil.
    private readonly ITodoService _todoService;

    /// <summary>
    /// Constructor donde el framework inyecta una implementación de ITodoService.
    /// Por qué: en la arquitectura hexagonal, el mesero solo conoce el MENÚ (interfaz),
    /// no la cocina concreta. El contenedor de DI trae la cocina real desde Program.cs.
    /// </summary>
    /// <param name="todoService">Instancia provista por el contenedor de DI.</param>
    public TodoController(ITodoService todoService)
    {
        // Guardamos la dependencia para usarla en cada "orden" (acción).
        _todoService = todoService;
    }

    // Acción HTTP GET sin parámetros: devuelve todas las tareas.
    // Flujo: Cliente -> Mesero (este método) -> Menú (ITodoService.GetAllTodos) -> Cocina (impl) -> Bibliotecario (DB) -> Respuesta.
    // Ruta efectiva: GET api/v1/Todo
    [HttpGet]
    public IActionResult GetAll()
    {
        var todoItems = _todoService.GetAllTodos();
        
        // Proceso de "Mapeo": Convertimos la lista de entidades internas (TodoItem)
        // a una lista de DTOs publicos (TodoItemDto).

        var todoDtos = todoItems.Select(item => new TodoItemDto
        {
            Id = item.Id,
            Title = item.Title,
            IsComplete = item.IsComplete
        }).ToList();
        
        return Ok(todoDtos);
    }

    // Acción HTTP GET con parámetro de ruta: obtiene una tarea por id.
    // Ruta efectiva: GET api/v1/Todo/{id}
    [HttpGet("{id}")]
    public IActionResult GetById(long id)
    {
        var todoItem = _todoService.GetTodoById(id);
        if (todoItem == null)
        {
            return NotFound();
        }
        
        // Mapeo del objeto encontrado a su version DTO.
        var todoDto = new TodoItemDto
        {
            Id = todoItem.Id,
            Title = todoItem.Title,
            IsComplete = todoItem.IsComplete
        };
            return Ok(todoDto);
    }

    // Acción HTTP POST: crea una nueva tarea.
    // Ruta efectiva: POST api/v1/Todo
    [HttpPost]
    public IActionResult Create([FromBody] TodoItem createDto)
    {
        // Mapeo inverso: Convertimos el DTO que llega del cliente a una entidad 'TodoItem'
        // que uestra logica de negocio y base de datos entienden.
        var todoItem = new TodoItem
        {
            Title = createDto.Title,
            IsComplete = createDto.IsComplete
        };

        var createdTodo = _todoService.CreateTodo(todoItem);
        
        // Creamos un DTO para la respuesta, para mostrarle al cliente el objeto completo con su nuevo Id.
        var responseDto = new TodoItemDto
        {
            Id = createdTodo.Id,
            Title = createdTodo.Title,
            IsComplete = createdTodo.IsComplete
        };

        return CreatedAtAction(nameof(GetById), new { id = responseDto.Id }, responseDto);

    }

    // Acción HTTP PUT: reemplaza el estado de una tarea existente.
    // Ruta efectiva: PUT api/v1/Todo/{id}
    [HttpPut("{id}")]
    public IActionResult Update(long id, [FromBody] TodoItem updatedDto)
    {
        // Para la actualizacion, no pasamos el DTO directamente al servicio.
        // Primero, obtenemos la entidad real de la base de datos.
        var existingTodo = _todoService.GetTodoById(id);
        if (existingTodo == null)
        {
            // Si no existe, 404.
            return NotFound();
        }
        
        // Actualizamos la entidad que ya existe con los nuevos datos del DTO.
        existingTodo.Title = updatedDto.Title;
        existingTodo.IsComplete = updatedDto.IsComplete;
        
        // Pasamos la entidad ya actualizada al servicio para que la guarde.
        _todoService.UpdateTodo(id, existingTodo);

        return NoContent(); // No content es la respuesta estandar para un PUT existoso.
    }

    // Acción HTTP DELETE: elimina una tarea por id.
    // Ruta efectiva: DELETE api/v1/Todo/{id}
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
