// Esta clase es la "cocina" del restaurante: cumple el contrato (ITodoService) y
// contiene la lógica real para preparar las órdenes (casos de uso).
// Mantiene el desacoplamiento delegando la persistencia al "bibliotecario" (TodoDbContext),
// quien sabe cómo leer y escribir en la base de datos. Así, el controlador no sabe nada de la DB.

using MiPrimeraAPI.application.Service;
using MiPrimeraAPI.domain.Entity;
using MiPrimeraAPI.infrastructure; // Importa el DbContext

namespace MiPrimeraAPI.domain
{
    public class TodoServiceImp : ITodoService
    {
        // Dependencia clave: el DbContext es nuestro bibliotecario experto.
        private readonly TodoDbContext _context;

        // Por qué DI aquí: recibimos el DbContext desde fuera para no acoplarnos a detalles de infraestructura.
        // Esto permite testear y cambiar la implementación sin tocar la lógica de negocio.
        public TodoServiceImp(TodoDbContext context)
        {
            _context = context;
        }

        // Lista todas las tareas: la cocina le pregunta al bibliotecario por todo el catálogo.
        public List<TodoItem> GetAllTodos()
        {
            return _context.TodoItems.ToList(); // Materializamos para no exponer IQueryable hacia capas superiores.
        }

        // Busca una tarea por id: si no está, devolvemos null para que el mesero responda 404.
        public TodoItem? GetTodoById(long id)
        {
            // Usamos .Find() que es eficiente para claves primarias.
            return _context.TodoItems.Find(id);
        }

        // Crea una nueva tarea: la cocina prepara el plato y pide al bibliotecario que lo registre.
        public TodoItem CreateTodo(TodoItem newTodo)
        {
            // 1) No calculamos el Id manualmente: la base lo autogenera.
            // 2) Añadimos la entidad a la mesa TodoItems.
            _context.TodoItems.Add(newTodo);
            // 3) Confirmamos la operación en la base (como sellar el pedido en el libro del bibliotecario).
            _context.SaveChanges();
            return newTodo;
        }

        // Actualiza una tarea existente: si no existe, no hacemos nada.
        public void UpdateTodo(long id, TodoItem updatedTodo)
        {
            var existingTodo = _context.TodoItems.Find(id);
            if (existingTodo != null)
            {
                // Decidimos qué campos actualizar (reglas de negocio simples).
                existingTodo.Title = updatedTodo.Title;
                existingTodo.IsComplete = updatedTodo.IsComplete;

                // Guardamos los cambios: el bibliotecario persiste el nuevo estado.
                _context.SaveChanges();
            }
        }

        // Elimina una tarea por id: retiramos el plato de la carta.
        public void DeleteTodo(long id)
        {
            var todoToRemove = _context.TodoItems.Find(id);
            if (todoToRemove != null)
            {
                // Quitamos el registro de la "mesa" TodoItems.
                _context.TodoItems.Remove(todoToRemove);
                // Confirmamos la eliminación en la base.
                _context.SaveChanges();
            }
        }
    }
}