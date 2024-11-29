using apiwork.Data;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using MySql.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

// Ajouter la configuration pour WorkConnectContext avec MySQL
builder.Services.AddDbContext<WorkConnectContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));


// Ajouter les services nécessaires pour l'API Web
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuration du middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
