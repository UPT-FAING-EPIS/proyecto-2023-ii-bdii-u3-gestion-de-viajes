using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using ClienteAPI.Models;

namespace ClienteAPI.Services
{
    public class RabbitMQService
    {
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQService(IConfiguration configuration)
        {
            _factory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMQ:HostName"],
                Port = Convert.ToInt32(configuration["RabbitMQ:Port"]),
                UserName = configuration["RabbitMQ:UserName"],
                Password = configuration["RabbitMQ:Password"]
            };

            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void Validar(string message, int op)
        {
            /* Console.WriteLine("MENSAJE RECIBIDO:" + message.ToString()); */
            message = "Se modifico " + message;
            Prueba test = new Prueba();
            test.Date = DateTime.Now;
            test.Text = message;
            
            
            switch(op){
                case 1:
                    test.Event = "GET";
                    break;
                case 2:
                    test.Event = "POST";
                    break;
                case 3:
                    test.Event = "PUT";
                    break;
                case 4:
                    test.Event = "DELETE";
                    break;
                default:
                    break;
            }
            if(test.Text!=null){
                
                SendMessage(test, "cola1");
            }

        }
        public void SendMessage<T>(T message, string queueName)
        {

            _channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            _channel.BasicPublish(exchange: "",
                                  routingKey: queueName,
                                  basicProperties: null,
                                  body: body);
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}