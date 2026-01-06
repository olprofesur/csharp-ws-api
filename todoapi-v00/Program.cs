using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Configuration;
using System.Diagnostics;
using System.Xml.Linq;
using TodoApi.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


var builder = WebApplication.CreateBuilder(args);

var cs = builder.Configuration.GetConnectionString("TodoDB")
         ?? "server=localhost;user id=todoapp;password=todoapp;database=todoapp";
builder.Services.AddDbContext<TodoContext>(options =>
    options.UseMySql(cs, ServerVersion.AutoDetect(cs)));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();   // /openapi/v1.json
    app.UseSwagger();   // /swagger/v1/swagger.json
    app.UseSwaggerUI(c => 
      {
             c.RoutePrefix = "swagger";
      }); // /swagger
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();



