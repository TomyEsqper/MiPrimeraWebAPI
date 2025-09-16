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
        // Llama a la capa de aplicación para obtener los datos.
        // Ok(...) genera respuesta 200 con el contenido serializado a JSON.
        return Ok(_todoService.GetAllTodos());
    }

    // Acción HTTP GET con parámetro de ruta: obtiene una tarea por id.
    // Ruta efectiva: GET api/v1/Todo/{id}
    [HttpGet("{id}")]
    public IActionResult GetById(long id)
    {
        // Recupera el recurso desde el servicio (la cocina pregunta al bibliotecario si existe).
        var todo = _todoService.GetTodoById(id);
        if (todo == null)
        {
            // Si no existe, responde 404 Not Found (no hay plato con ese id).
            return NotFound();
        }

        // Si existe, responde 200 con el recurso.
        return Ok(todo);
    }

    // Acción HTTP POST: crea una nueva tarea.
    // Importante: ahora usamos un DTO (TodoCreateRequest) específico de entrada con validaciones.
    // Por qué: el "bouncer" (validación de modelo de [ApiController]) puede revisar el pedido ANTES de llegar a la cocina.
    // Ruta efectiva: POST api/v1/Todo
    [HttpPost]
    public IActionResult Create([FromBody] TodoCreateRequest request)
    {
        // Con [ApiController], si request no cumple las DataAnnotations, ASP.NET Core devuelve 400 automáticamente
        // con detalles de qué falló. Aquí ya llegamos con un pedido válido.
        // Mapeamos el DTO (capa transporte) a la entidad de dominio que entiende la cocina (servicio).
        var newTodo = new TodoItem
        {
            Title = request.Title,
            IsComplete = request.IsComplete
        };

        // Delegamos la creación al servicio (la cocina); este puede asignar Id y pedir al bibliotecario que lo guarde.
        var createdTodo = _todoService.CreateTodo(newTodo);

        // Devuelve 201 Created con cabecera Location apuntando al recurso recién creado.
        // CreatedAtAction usa la acción GetById y pasa el id del nuevo recurso en la ruta.
        return CreatedAtAction(nameof(GetById), new { id = createdTodo.Id }, createdTodo);
    }

    // Acción HTTP PUT: reemplaza el estado de una tarea existente.
    // Ruta efectiva: PUT api/v1/Todo/{id}
    [HttpPut("{id}")]
    public IActionResult Update(long id, [FromBody] TodoItem updatedTodo)
    {
        // Verifica que el recurso exista antes de intentar modificarlo.
        var existingTodo = _todoService.GetTodoById(id);
        if (existingTodo == null)
        {
            // Si no existe, 404.
            return NotFound();
        }

        // Aplica los cambios en la capa de aplicación (la cocina decide cómo actualizar y pide al bibliotecario persistir).
        _todoService.UpdateTodo(id, updatedTodo);

        // 204 No Content indica éxito sin cuerpo de respuesta.
        return NoContent();
    }

    // Acción HTTP DELETE: elimina una tarea por id.
    // Ruta efectiva: DELETE api/v1/Todo/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(long id)
    {
        // Confirmamos que el recurso exista (no intentamos borrar un plato que no está en la carta).
        var existingTodo = _todoService.GetTodoById(id);
        if (existingTodo == null)
        {
            // Si no existe, 404.
            return NotFound();
        }

        // Solicitamos la eliminación al servicio (la cocina coordina con el bibliotecario para quitarlo de los registros).
        _todoService.DeleteTodo(id);

        // 204 No Content confirma eliminación exitosa.
        return NoContent();
    }
}



