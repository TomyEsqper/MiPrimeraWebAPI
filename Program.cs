// En este archivo arranca toda la aplicación web.
// Piensa en la API como un restaurante montado dentro de un castillo (arquitectura hexagonal).
// Aquí encendemos las luces, contratamos al mesero (Controllers), conectamos la cocina (Servicios) y
// presentamos al bibliotecario experto (DbContext) que sabe hablar con la base de datos.
// La Inyección de Dependencias (DI) es el “pegamento” que mantiene todo desacoplado:
// cada pieza conoce solo “contratos” (interfaces), no implementaciones concretas.

using MiPrimeraAPI.application.Service; // Trae la interfaz del servicio (el "menú" que define QUE se puede hacer).
using MiPrimeraAPI.domain;               // Trae la implementación del servicio (la "cocina" real).
using MiPrimeraAPI.infrastructure;       // Trae el DbContext (el "bibliotecario" que habla con la base de datos).
using Microsoft.EntityFrameworkCore;     // Proveedor de EF Core para usar SQLite, migraciones, etc.

var builder = WebApplication.CreateBuilder(args); // Crea el "arquitecto" de la app: configura servicios y el pipeline HTTP.

// Registramos el bibliotecario (DbContext) en el contenedor de DI.
// Por qué: el DbContext es el "experto en libros" que se encarga de consultar/guardar datos.
// Usamos UseSqlite con la cadena de conexión definida en appsettings.* para apuntar a la base de datos concreta.
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registramos los controladores (los "meseros").
// Por qué: esta capa es la ÚNICA que habla con el mundo exterior (HTTP).
// Además, [ApiController] habilita validaciones automáticas de modelo y respuestas 400 por defecto.
builder.Services.AddControllers();

// Registramos el servicio de dominio a través de su CONTRATO (ITodoService -> TodoServiceImp).
// Por qué: esto es la esencia del desac acoplamiento en el castillo hexagonal.
// El controlador solo conoce el "menú" (ITodoService); el runtime le inyecta la "cocina" real (TodoServiceImp).
builder.Services.AddScoped<ITodoService, TodoServiceImp>();

// Exponemos endpoints para herramientas como Swagger (documentación y pruebas).
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Genera la UI y el documento OpenAPI (como la carta visible del restaurante).

var app = builder.Build(); // Construye la aplicación con todo lo registrado.

// En desarrollo, abrimos la carta interactiva (Swagger) para explorar y probar el restaurante.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();    // Genera el documento OpenAPI
    app.UseSwaggerUI();  // Muestra la UI de Swagger
}

// Middleware que redirige HTTP a HTTPS.
// Por qué: seguridad básica; evita que la petición viaje "sin candado" por la red.
app.UseHttpsRedirection();

// Este paso conecta los "meseros" (Controllers) al pipeline HTTP.
// Por qué: aquí el castillo abre sus puertas (rutas) para que entren las solicitudes.
app.MapControllers();

// Enciende el servidor y se queda escuchando como la puerta principal del castillo.
app.Run();