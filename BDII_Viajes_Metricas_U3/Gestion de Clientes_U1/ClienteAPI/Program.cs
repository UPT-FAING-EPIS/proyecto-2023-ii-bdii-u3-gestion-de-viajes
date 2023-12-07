using AutoMapper;
using ClienteAPI;
using ClienteAPI.Data;
using ClienteAPI.Models;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using ClienteAPI.Services;
using Newtonsoft.Json;
//
using Prometheus;
using ClienteAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<BdClientesContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("ClienteDB")));
/* builder.Services.AddSingleton<RabbitMQService>(); */

// vars RABBITMQ

/* var rabbitMQConfig = configuration.GetSection("RabbitMQ");
var rabbitMQHostName = rabbitMQConfig["HostName"];
var rabbitMQPort = int.Parse(rabbitMQConfig["Port"]);
var rabbitMQUserName = rabbitMQConfig["UserName"];
var rabbitMQPassword = rabbitMQConfig["Password"];
var rabbitMQExchangeName = rabbitMQConfig["ExchangeName"];

builder.Services.AddSingleton<IRabbitMQService>(sp =>
{
    return new RabbitMQService(rabbitMQHostName, rabbitMQPort, rabbitMQUserName, rabbitMQPassword, rabbitMQExchangeName);
}); */


//

builder.Services.AddControllers().AddFluentValidation(fv =>
{
    fv.RegisterValidatorsFromAssemblyContaining<Program>();

});

builder.Services.AddScoped<RabbitMQService>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// **************************MAPPER
var mapperConfig = new MapperConfiguration(m => 
{
    m.AddProfile(new MappingProfile());
});


IMapper mapper = mapperConfig.CreateMapper();

builder.Services.AddSingleton(mapper);
builder.Services.AddMvc();


// Obtén la configuración de RabbitMQ
/* var rabbitMQConfig = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMQConfig>(); */


// REACTIVAR ESTO
// Agrega el servicio RabbitMQConnection

/* builder.Services.AddSingleton<RabbitMQConnection>();

builder.Services.AddSingleton<RabbitMQClient>(); */

// Agrega el cliente RabbitMQ a los servicios
/* builder.Services.AddSingleton(rabbitMQConfig); */

// FIN RABBITMQ

var app = builder.Build();

//PROMETHEUS-METRICAS-PROMETHEUS-METRICAS-PROMETHEUS-METRICAS-PROMETHEUS-METRICAS-PROMETHEUS-METRICAS-PROMETHEUS-METRICAS-PROMETHEUS-METRICAS

app.UseRouting();//Configuracion de enrutamiento
app.UseHttpMetrics();//Metricas HTTP básicas
app.UseMiddleware<PrometheusMetricsMiddleware>();//Metricas personalizadas
app.UseMetricServer();//Exponer las metricas

//PROMETHEUS-METRICAS-PROMETHEUS-METRICAS-PROMETHEUS-METRICAS-PROMETHEUS-METRICAS-PROMETHEUS-METRICAS-PROMETHEUS-METRICAS-PROMETHEUS-METRICAS

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment() )
// {
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


// Ejemplo de publicación de mensaje con RabbitMQ
/* using var scope = app.Services.CreateScope();
var serviceProvider = scope.ServiceProvider;
var rabbitMQClient = serviceProvider.GetRequiredService<RabbitMQClient>();

var message = "TESTESTTESTTEST"; */
/* rabbitMQClient.PublishMessage(message); */

/* var jsonString = prueba();
static string prueba()
    {
        var myMessage = new
        {
            Text = "PRUEBA NO VALIDA",
            Date = DateTime.Parse("2023-11-11")
        };

        // Serializa el objeto anónimo a una cadena JSON
        Console.WriteLine($"JSON DE PRUEBA: {myMessage}");
        var jsonString = JsonConvert.SerializeObject(myMessage);
        
        // Devuelve la cadena JSON
        return jsonString;
    } */

/* rabbitMQClient.PublishMessage(jsonString); */





/* 
app.MapPost("/cliente/", async (Cliente a, BdClientesContext db, IRabbitMQService rabbitMQService) =>
{
    db.Asistencias.Add(a);
    await db.SaveChangesAsync();
    var logMessage = $"New Asistencia added: {a.Id}";
    rabbitMQService.SendMessage(logMessage);

    return Results.Created($"/asistencia/{a.Id}", a);
}); */


app.Run();
