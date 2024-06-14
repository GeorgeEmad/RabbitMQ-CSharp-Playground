
using RabbitMQ.Client;
using System.Text;

var connectionFactory = new ConnectionFactory{
    Uri = new Uri("amqp://guest:guest@localhost:5672"),
    ClientProvidedName = "Producer Topic Exchange"
};
using var connection = connectionFactory.CreateConnection();
using var channel = connection.CreateModel();
channel.ExchangeDeclare(exchange: "TopicExchange", type: ExchangeType.Topic);

var message = "This message is published to Using Topic Exchange: user.egypt.purchase";
var encodedMessage = Encoding.UTF8.GetBytes(message);
channel.BasicPublish("TopicExchange", "user.egypt.purchase", null, encodedMessage);

Console.WriteLine(message);

message = "This message is published to Using Topic Exchange: user.france.sell";
encodedMessage = Encoding.UTF8.GetBytes(message);
channel.BasicPublish("TopicExchange", "user.france.sell", null, encodedMessage);

Console.WriteLine(message);

message = "This message is published to Using Topic Exchange: user.egypt.order";
encodedMessage = Encoding.UTF8.GetBytes(message);
channel.BasicPublish("TopicExchange", "user.egypt.order", null, encodedMessage);



Console.WriteLine(message);
