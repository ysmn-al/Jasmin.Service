using Jasmin.Db;
using Jasmin.Db.DB;
using Jasmin.Host;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Jasmin.Service.Services;
using Microsoft.Extensions.Logging;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;

// Здесь добавляем CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:5002") // Укажите разрешенные источники
               .AllowAnyHeader() // Разрешить любые заголовки
               .AllowAnyMethod(); // Разрешить любые HTTP-методы
    });
});

builder.Services.AddApplicationDependencies()
    .AddDbDependencies(configuration);

var dbUpdater = new JasminDBMigration(configuration.GetConnectionString(nameof(ConnectionStrings.ConnectionString)));
dbUpdater.ApplyMigrations();

builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var initialDataService = services.GetRequiredService<InitialDataService>();
        await initialDataService.Init(); // Инициализация данных
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

if (app.Environment.IsDevelopment()) // by default enabled only for dev.
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

// Применение CORS перед авторизацией и маршрутизацией
app.UseCors("MyPolicy");

app.UseAuthorization();
app.MapControllers();

app.Run();