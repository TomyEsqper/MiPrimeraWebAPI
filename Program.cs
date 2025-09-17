// Importamos todas las herramientas y servicios que vamos a necesitar.
using MiPrimeraAPI.application.Service;
using MiPrimeraAPI.domain;
using MiPrimeraAPI.infrastructure;
using Microsoft.EntityFrameworkCore;

// --- PASO 1: Crear el constructor de la aplicación ---
var builder = WebApplication.CreateBuilder(args);


// --- PASO 2: Registrar todos los servicios en el contenedor ---

// Registro del DbContext: Le enseña a la app cómo hablar con la base de datos.
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registro de nuestro servicio de aplicación: Conecta la interfaz con su implementación.
builder.Services.AddScoped<ITodoService, TodoServiceImp>();

// Servicios estándar para que una API funcione con Controladores y Swagger.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// --- PASO 3: Construir y configurar la aplicación ---
var app = builder.Build();

// Configuración para que Swagger solo funcione en el entorno de desarrollo.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirección a HTTPS para seguridad.
app.UseHttpsRedirection();

// Activa el sistema para que la app use los Controladores que hemos creado.
app.MapControllers();


// --- PASO 4: ¡Arrancar la aplicación! ---
app.Run();