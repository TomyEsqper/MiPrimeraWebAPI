using Microsoft.AspNetCore.Mvc; // Importa atributos y tipos base para construir controladores y manejar HTTP.
using MiPrimeraAPI.application.Service; // El controlador conoce solamente el CONTRATO (interfaz) del servicio.
using MiPrimeraAPI.domain.Entity; // Importa el modelo de datos que llega/sale en el cuerpo de las peticiones.

namespace MiPrimeraAPI.adapter.restful.v1.controller; // Agrupa las clases en un espacio lógico y controla el nombre totalmente calificado.

// ApiController habilita funciones automáticas: validación de modelo, binding y respuestas 400 por defecto.
[ApiController]
// Define el patrón de ruta base para todas las acciones de este controlador.
// [controller] se reemplaza por "Todo" (nombre de la clase sin "Controller").
// Ejemplo: https://localhost:{puerto}/api/v1/Todo
[Route("api/v1/[controller]")]
public class TodoController : ControllerBase // Hereda utilidades para construir respuestas HTTP (Ok, NotFound, etc.).
{
    // Dependencia principal del controlador: contrato del servicio de tareas.
    // readonly: se asigna en el constructor y no cambia su referencia.
    private readonly ITodoService _todoService;

    /// <summary>
    /// Constructor donde el framework inyecta una implementación de ITodoService.
    /// Este patrón desacopla el controlador de la implementación concreta.
    /// </summary>
    /// <param name="todoService">Instancia provista por el contenedor de DI.</param>
    public TodoController(ITodoService todoService)
    {
        // Asignamos la dependencia inyectada al campo privado para usarla en las acciones.
        _todoService = todoService;
    }

    // Acción HTTP GET sin parámetros: devuelve todas las tareas.
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
        // Recupera el recurso desde el servicio.
        var todo = _todoService.GetTodoById(id);
        if (todo == null)
        {
            // Si no existe, responde 404 Not Found.
            return NotFound();
        }

        // Si existe, responde 200 con el recurso.
        return Ok(todo);
    }

    // Acción HTTP POST: crea una nueva tarea.
    // El cuerpo de la petición (JSON) se mapea a TodoItem por [FromBody].
    // Ruta efectiva: POST api/v1/Todo
    [HttpPost]
    public IActionResult Create([FromBody] TodoItem newTodo)
    {
        // Delegamos la creación al servicio (que asigna Id y persiste en la "DB" en memoria).
        var createdTodo= _todoService.CreateTodo(newTodo);
        // Devuelve 201 Created con cabecera Location apuntando al recurso recién creado.
        // CreatedAtAction usa la acción GetById y pasa el id del nuevo recurso en la ruta.
        return CreatedAtAction(nameof(GetById), new {id = createdTodo.Id}, createdTodo);
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

        // Aplica los cambios en la capa de aplicación.
        _todoService.UpdateTodo(id,updatedTodo);
        // 204 No Content indica éxito sin cuerpo de respuesta.
        return NoContent();
    }

    // Acción HTTP DELETE: elimina una tarea por id.
    // Ruta efectiva: DELETE api/v1/Todo/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(long id)
    {
        // Confirmamos que el recurso exista.
        var existingTodo = _todoService.GetTodoById(id);
        if (existingTodo == null)
        {
            // Si no existe, 404.
            return NotFound();
        }

        // Solicitamos la eliminación al servicio.
        _todoService.DeleteTodo(id);
        // 204 No Content confirma eliminación exitosa.
        return NoContent();
    }
}



