// Esta interfaz es el CONTRATO de la capa de aplicación en la arquitectura hexagonal.
// Piensa en ella como el MENÚ del restaurante: aquí se declara QUÉ se puede pedir,
// pero NO CÓMO se prepara. El controlador (mesero) solo habla con este contrato,
// y la implementación real (la cocina) se inyecta en tiempo de ejecución via DI.


// Importa el modelo de dominio que se usa en las firmas del contrato.
using MiPrimeraAPI.domain.Entity; // Necesitamos TodoItem para declarar los métodos.

namespace MiPrimeraAPI.application.Service; // Capa de aplicación: define contratos y casos de uso.


// La interfaz define QUÉ operaciones existen para trabajar con TodoItem,
// sin fijar CÓMO se implementan. Permite desacoplar controlador e implementación.
public interface ITodoService
{
    // Por qué: listar todos los platos (tareas) disponibles para mostrarlos al cliente.
    // La capa web lo usa en GET api/v1/Todo.
    List<TodoItem> GetAllTodos();   

    // Por qué: consultar un plato específico por su id. Devuelve null si no existe para que el mesero responda 404.
    // Se usa en GET api/v1/Todo/{id}.
    TodoItem? GetTodoById(long id);

    // Por qué: dar de alta un nuevo plato en la carta. La cocina asigna Id y coordina con el bibliotecario para persistir.
    // Se usa en POST api/v1/Todo.
    TodoItem CreateTodo(TodoItem newTodo);

    // Por qué: actualizar el estado/atributos de un plato ya existente.
    // La cocina valida existencia y guarda cambios. Se usa en PUT api/v1/Todo/{id}.
    void UpdateTodo(long id, TodoItem updatedTodo); // 'void' = solo efectúa la acción.

    // Por qué: retirar un plato de la carta (eliminar la tarea) por id. Se usa en DELETE api/v1/Todo/{id}.
    void DeleteTodo(long id);
}

