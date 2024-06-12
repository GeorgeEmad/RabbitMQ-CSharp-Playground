using System;
using System.Threading.Tasks;
using RabbitMQ.Client;
using System.Collections;
using System.Text;

var connectionFactory = new ConnectionFactory{
    Uri = new Uri("amqp://guest:guest@localhost:5672"),
    ClientProvidedName = "Producer Fanout"
};
using var connection = connectionFactory.CreateConnection();
using var channel = connection.CreateModel();
channel.ExchangeDeclare(exchange: "FanOutExchange", type: ExchangeType.Fanout);

var message = "This message is published to all in pub/sub pattern";
var encodedMessage = Encoding.UTF8.GetBytes(message);

channel.BasicPublish("FanOutExchange", "", null, encodedMessage);

Console.WriteLine(message);
