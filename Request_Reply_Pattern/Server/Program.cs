using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory{
    Uri = new Uri("amqp://guest:guest@localhost:5672"),
    ClientProvidedName = "Server"
};
var connection = factory.CreateConnection();
var channel = connection.CreateModel();

channel.QueueDeclare(queue: "request-queue", exclusive: false);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, EventArgs) =>
{
    var message = EventArgs.Body.ToArray();
    var decodedMessage = Encoding.UTF8.GetString(message);
    Console.WriteLine($"Recieved RRequest: {decodedMessage}");
    var properties = channel.CreateBasicProperties();
    properties.CorrelationId = EventArgs.BasicProperties.CorrelationId;

    var replyMessage = "Reply message";
    var encodedMessage = Encoding.UTF8.GetBytes(replyMessage);
    channel.BasicPublish("", routingKey: EventArgs.BasicProperties.ReplyTo, basicProperties:properties, encodedMessage);
    Console.WriteLine($"Reply Sent: {replyMessage}");

};
channel.BasicConsume(queue:"request-queue", autoAck: false,  consumer);


Console.WriteLine("Enter to Exit");
Console.ReadLine();


