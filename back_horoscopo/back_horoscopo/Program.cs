using AutoMapper;
using back_horoscopo.Application.DTOs;
using back_horoscopo.Application.Interface;
using back_horoscopo.Application.Mappers;
using back_horoscopo.Application.Services;
using back_horoscopo.Infrastructure.Implementation;
using back_horoscopo.Infrastructure.Interface;
using back_horoscopo.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<HoroscopoContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("conexion"), Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.37-mysql"))
);
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<IHoroscopoService, HoroscopoService>();
builder.Services.AddScoped<MapperUsuario>();
builder.Services.AddScoped<IUsuarioData, UsuarioData>();
builder.Services.AddScoped<IEstadisticaData, EstadisticaData>();
builder.Services.AddScoped<IConsultaData, ConsultaData>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Cambia al dominio de tu frontend
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigins");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
