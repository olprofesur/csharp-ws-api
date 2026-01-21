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
    options.AddDefaultPolicy(policy =>
        policy
            .SetIsOriginAllowed(_ => true) // accetta tutto
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
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

app.UseCors();

app.UseAuthentication(); 

app.UseAuthorization();

app.MapControllers();

app.Run();






