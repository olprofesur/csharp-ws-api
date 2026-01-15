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
builder.Services.AddDbContext<UserContext>(options =>
    options.UseMySql(cs, ServerVersion.AutoDetect(cs)));

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy
            .SetIsOriginAllowed(origin =>
            {
                // Permetti Live Server in locale
                if (origin == "http://localhost:5500" || origin == "http://127.0.0.1:5500")
                    return true;

                // Permetti i preview/forward di Codespaces (frontend su porta qualsiasi)
                // es: https://<qualcosa>-5500.app.github.dev
                if (Uri.TryCreate(origin, UriKind.Absolute, out var uri))
                    return uri.Host.EndsWith(".app.github.dev", StringComparison.OrdinalIgnoreCase);

                return false;
            })
            .AllowAnyHeader()
            .AllowAnyMethod()
    );
});


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.RoutePrefix = "swagger"; });
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("Frontend");

app.UseAuthentication(); 

app.UseAuthorization();

app.MapControllers();

app.Run();






