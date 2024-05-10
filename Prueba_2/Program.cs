using Microsoft.EntityFrameworkCore;
using Prueba_2.Controllers;
using Prueba_2.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<TestContext>( opts => opts.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseString"))); // config cadena de conexión db
builder.Services.AddControllers();
builder.Services.AddScoped<EmpleadoService>(); // Empleado service

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
