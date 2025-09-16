using AutoMapper;
using MiPrimeraAPI.adapter.restful.v1.dto;
using MiPrimeraAPI.domain.Entity;

namespace MiPrimeraAPI;

// Este archivo es el "diccionario" o el "entrenamiento intensivo" que le damos a nuestro traductor universal (AutoMapper).
// Aquí le enseñamos cómo convertir cada tipo de "menú" (DTO) en su "receta secreta" (entidad de dominio) y viceversa.
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Traducción de la receta secreta (TodoItem) al menú público (TodoItemDto) para mostrar al cliente.
        CreateMap<TodoItem, TodoItemDto>();
        // Traducción de la orden del cliente (CreateTodoDto) a la receta secreta (TodoItem) que entiende la cocina.
        CreateMap<CreateTodoDto, TodoItem>();
    }
}