// Este DTO representa el "pedido" que llega por la puerta del castillo (HTTP) para crear una tarea.
// Por qué un DTO y no usar directamente la entidad de dominio:
//   - Separación de responsabilidades: la forma en que entra el pedido puede cambiar sin afectar al dominio.
//   - Validación en el borde: aquí ponemos al "bouncer" a revisar que el título no esté vacío.
//   - Documentación clara: Swagger mostrará exactamente qué se espera en el POST.

using System.ComponentModel.DataAnnotations;

namespace MiPrimeraAPI.adapter.restful.v1.dto
{
    public class TodoCreateRequest
    {
        // Título obligatorio: no puede ser nulo, vacío ni solo espacios.
        // [ApiController] hace que si esto no se cumple, la API responda 400 automáticamente con detalles.
        [Required(ErrorMessage = "El título es obligatorio.")]
        [MinLength(1, ErrorMessage = "El título no puede estar vacío.")]
        [RegularExpression(@".*\S.*", ErrorMessage = "El título no puede ser solo espacios en blanco.")]
        public string Title { get; set; } = string.Empty;
// Este DTO representa el "pedido" que llega por la puerta del castillo (HTTP) para crear una tarea.
// Por qué un DTO en lugar de usar directamente la entidad de dominio (TodoItem):
//   - Separación de responsabilidades: el formato de entrada/salida de la API (transporte) puede cambiar sin afectar al dominio.
//   - Validación en el borde (anti-corrupción): aquí ponemos al "bouncer" a revisar que el título no esté vacío.
//   - Documentación clara: Swagger mostrará exactamente qué se espera en la operación POST.

using System.ComponentModel.DataAnnotations;

namespace MiPrimeraAPI.adapter.restful.v1.dto
{
    public class TodoCreateRequest
    {
        // Título obligatorio: no puede ser nulo, vacío ni solo espacios.
        // [ApiController] hace que si esto no se cumple, la API responda 400 automáticamente con detalles.
        [Required(ErrorMessage = "El título es obligatorio.")]
        [MinLength(1, ErrorMessage = "El título no puede estar vacío.")]
        [RegularExpression(@".*\S.*", ErrorMessage = "El título no puede ser solo espacios en blanco.")]
        public string Title { get; set; } = string.Empty;

        // Indicador de completado; puede llegar como false por defecto si el cliente no lo envía.
        public bool IsComplete { get; set; }
    }
}
        // Indicador de completado; puede llegar como false por defecto si el cliente no lo envía.
        public bool IsComplete { get; set; }
    }
}
