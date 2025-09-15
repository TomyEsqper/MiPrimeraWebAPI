// ------------------------------------------------------------------------------------
// PASO 1: IMPORTAR LAS HERRAMIENTAS QUE NECESITAMOS
// ------------------------------------------------------------------------------------
// Estos 'using' dicen: "Vamos a usar tipos y servicios definidos en estos namespaces".
using MiPrimeraAPI.application.Service; // Para registrar e inyectar ITodoService en el contenedor DI.
using MiPrimeraAPI.domain;              // Para resolver TodoServiceImp como implementación concreta.


// ------------------------------------------------------------------------------------
// PASO 2: CONFIGURAR LOS SERVICIOS DE NUESTRA APLICACIÓN
// ------------------------------------------------------------------------------------
// 'builder' crea y configura la aplicación, leyendo args de la CLI (puerto, entorno, etc.).
var builder = WebApplication.CreateBuilder(args);

// Registramos el soporte para controladores (atributos [ApiController], binding de modelos, etc.).
builder.Services.AddControllers();

// Inyección de Dependencias (DI):
// - AddScoped: una instancia por petición HTTP.
// - Cuando alguien requiera ITodoService, se entregará TodoServiceImp.
builder.Services.AddScoped<ITodoService, TodoServiceImp>();

// Soporte para Swagger (descubrimiento de endpoints + UI de prueba).
builder.Services.AddEndpointsApiExplorer(); // Expone metadatos de endpoints para OpenAPI.
builder.Services.AddSwaggerGen();           // Genera el documento OpenAPI y la UI.


// ------------------------------------------------------------------------------------
// PASO 3: CONSTRUIR Y EJECUTAR LA APLICACIÓN
// ------------------------------------------------------------------------------------
// Construimos la app con todo lo anterior registrado.
var app = builder.Build();

// Middleware de desarrollo: expone Swagger solo en Development.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();    // Expone /swagger/v1/swagger.json (documento OpenAPI).
    app.UseSwaggerUI();  // Habilita la interfaz web de Swagger en /swagger.
}

// Redirige HTTP a HTTPS para mejorar la seguridad del transporte.
app.UseHttpsRedirection();

// Mapea automáticamente los controladores y sus rutas para que empiecen a responder.
app.MapControllers();

// Arranca el servidor Kestrel y comienza a escuchar solicitudes entrantes.
app.Run();
