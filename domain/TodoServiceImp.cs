using System.Linq; // Extensiones LINQ para consultas a colecciones (FirstOrDefault, Any, Max).
using MiPrimeraAPI.application.Service; // Importa el contrato que vamos a cumplir (ITodoService).
using MiPrimeraAPI.domain.Entity;       // Importa el modelo de Tarea (TodoItem).

namespace MiPrimeraAPI.domain; // Capa de dominio: implementación concreta de reglas/servicios.

// Implementación concreta del contrato ITodoService para gestionar tareas.
public class TodoServiceImp : ITodoService
{
    // "Base de datos" en memoria para datos de demostración.
    // private: solo esta clase puede acceder.
    // static: única instancia compartida durante la vida de la app.
    // readonly: no se puede reasignar la lista (sí modificar su contenido).
    private static readonly List<TodoItem> _memoriaDB = new List<TodoItem>
    {
        // Datos de ejemplo para iniciar y probar el flujo end-to-end.
        new TodoItem { Id = 1, Title = "Seguir el ejemplo del profe", IsComplete = true },
        new TodoItem { Id = 2, Title = "Reorganizar carpetas", IsComplete = false },
        new TodoItem { Id = 3, Title = "Probar la API en Swagger", IsComplete = false },
        // Entradas duplicadas a propósito para ejercicios de prueba.
        new TodoItem { Id = 1, Title = "Entender el problema de nombres", IsComplete = true },
        new TodoItem { Id = 2, Title = "Reemplazar los archivos", IsComplete = true },
        new TodoItem { Id = 3, Title = "Probar en Swagger OTRA VEZ!", IsComplete = false }
    };

    // Devuelve todas las tareas almacenadas.
    public List<TodoItem> GetAllTodos()
    {
        // En una app real, aquí se consultaría una base de datos o un repositorio.
        return _memoriaDB; 
    }

    // Busca un elemento por Id; puede devolver null si no existe.
    public TodoItem? GetTodoById(long id)
    {
        // LINQ recorre la colección y devuelve el primer elemento que cumpla la condición.
        return _memoriaDB.FirstOrDefault(todo => todo.Id == id);
    }

    // Crea una nueva tarea asignándole un Id incremental.
    public TodoItem CreateTodo(TodoItem newTodo)
    {
        // Calcula el próximo Id basado en el máximo actual; si la lista está vacía, usa 1.
        var newId = _memoriaDB.Any() ?  _memoriaDB.Max(x => x.Id) + 1 : 1;
        // Asigna el Id al nuevo elemento recibido.
        newTodo.Id = newId;
        // Inserta el elemento en la "DB" en memoria.
        _memoriaDB.Add(newTodo);
        // Devuelve el elemento creado (incluye su Id).
        return newTodo;
    }

    // Actualiza los campos mutables del elemento identificado por 'id'.
    public void UpdateTodo(long id, TodoItem updatedTodo)
    {
        // Busca el elemento existente.
        var existingTodo = GetTodoById(id);
        if (existingTodo != null)
        {
            // Copia los valores actualizados.
            existingTodo.Title = updatedTodo.Title;
            existingTodo.IsComplete = updatedTodo.IsComplete;
        }
    }

    // Elimina el elemento por id (si existe).
    public void DeleteTodo(long id)
    {
        // Busca el elemento objetivo.
        var todoToRemove = GetTodoById(id);
        if (todoToRemove != null)
        {
            // Remueve de la colección en memoria.
            _memoriaDB.Remove(todoToRemove);
        }
    }
}
