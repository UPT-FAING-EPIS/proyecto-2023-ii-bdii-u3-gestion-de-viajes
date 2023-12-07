var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddSingleton<RabbitMQClientService>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    return new RabbitMQClientService(configuration);
});

builder.Services.AddHostedService<RabbitMQConsumerService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();