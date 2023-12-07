using RabbitMQ.Client;
using System;
using System.Text;

public class RabbitMQClientService : IDisposable
{
    private readonly IModel _channel;
    private readonly IConnection _connection;
    private readonly string _queueName = "cola1";

    public RabbitMQClientService(IConfiguration configuration)
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

        _channel.QueueDeclare(queue: _queueName,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
    }

    public void Publish(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(exchange: "",
                             routingKey: _queueName,
                             basicProperties: null,
                             body: body);
    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}
