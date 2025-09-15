// Importa el modelo de dominio que se usa en las firmas del contrato.
using MiPrimeraAPI.domain.Entity; // Necesitamos TodoItem para declarar los métodos.

namespace MiPrimeraAPI.application.Service; // Capa de aplicación: define contratos y casos de uso.

// La interfaz define QUÉ operaciones existen para trabajar con TodoItem,
// sin fijar CÓMO se implementan. Permite desacoplar controlador e implementación.
public interface ITodoService
{
    // Recupera todas las tareas (operación de lectura).
    List<TodoItem> GetAllTodos();   

    // Busca una tarea por identificador; null si no existe.
    TodoItem? GetTodoById(long id);

    // Crea una nueva tarea y la devuelve con su Id asignado.
    TodoItem CreateTodo(TodoItem newTodo);

    // Actualiza los campos de una tarea existente; no retorna contenido.
    void UpdateTodo(long id, TodoItem updatedTodo); // 'void' = solo efectúa la acción.

    // Elimina una tarea por identificador.
    void DeleteTodo(long id);
}

