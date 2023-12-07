using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using MongoDB.Driver;
using System;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using Bitacora.Services.RabbitMq;
public class RabbitMQConsumerService : BackgroundService
{
    private readonly IModel _channel;
    private readonly IConnection _connection;
    private readonly ILogger<RabbitMQConsumerService> _logger;
    private readonly string _queueName = "cola1";
    private readonly IMongoCollection<BsonDocument> _mongoCollection;

    public RabbitMQConsumerService(IConfiguration configuration, ILogger<RabbitMQConsumerService> logger)
    {
        var factory = new ConnectionFactory()
        {
            HostName = configuration["RabbitMQ:HostName"],
            Port = Convert.ToInt32(configuration["RabbitMQ:Port"]),
            UserName = configuration["RabbitMQ:UserName"],
            Password = configuration["RabbitMQ:Password"]
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _logger = logger;

        _channel.QueueDeclare(queue: _queueName,
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var mongoClient = new MongoClient(configuration["MongoDB:ConnectionString"]);
        var mongoDatabase = mongoClient.GetDatabase(configuration["MongoDB:DatabaseName"]);
        _mongoCollection = mongoDatabase.GetCollection<BsonDocument>(configuration["MongoDB:CollectionName"]);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
{
    var body = ea.Body.ToArray();
    var messageJson = Encoding.UTF8.GetString(body);
   _logger.LogInformation($"Received raw message: {messageJson}");

    try
    {
        var messageObject = JsonConvert.DeserializeObject<RabbitMessage>(messageJson);
        var document = new BsonDocument
        {
            { "event", messageObject.Text },
            { "date", messageObject.Date },
            { "received_at", DateTime.UtcNow }
        };
        await _mongoCollection.InsertOneAsync(document);
    }
    catch (JsonException ex)
    {
        _logger.LogError(ex, "Error al deserializar el mensaje recibido");
    }
};

        _channel.BasicConsume(queue: _queueName,
                             autoAck: true,
                             consumer: consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
        base.Dispose();
    }
}
