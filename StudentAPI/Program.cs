using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StudentAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

builder.Services.AddControllers(); // Use controllers to handle web requests
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection of DbContext Class (how to communicate with the database)
builder.Services.AddDbContext<APIDbContext>(options => // Method to create instances of DbContext class
    options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection"))); 

var app = builder.Build(); // Create app with provided settings

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    // Turn on SwaggerUI when testing (IsDevelopment() == True)
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization(); // Checks user permissions
app.MapControllers(); // Connects webURL to the code that handles it
app.Run(); // Everything get started


// Note to self: An instance of the DbContext class is an object stored in RAM that translates
// C# code into code SQL can understand