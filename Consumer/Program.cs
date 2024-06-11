using System;
using System.Text;
using System.Collections;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using System.Threading.Tasks;

var connectionFactory = new ConnectionFactory{
    Uri = new Uri("amqp:guest:quest@localhost:5672"),
    ClientProvidedName = "Producer App" 
};

var connecion = connectionFactory.CreateConnection();
var channel = connecion.CreateModel();
channel.QueueDeclare(
    queue: "letterboxtest",
    durable: false,
    exclusive: false,
    autoDelete:false,
    arguments: null
);

channel.BasicQos(0, 1,  false);

var random = new Random();
var eventBasicConsumer = new EventingBasicConsumer(channel);
eventBasicConsumer.Received += (model, eventArguments) =>{
    var processingTime = random.Next(1,5);
    var body = eventArguments.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Task.Delay(TimeSpan.FromSeconds(processingTime)).Wait();
    channel.BasicAck(eventArguments.DeliveryTag, multiple: false);
    Console.WriteLine($"message: {message} was processed in {processingTime}s");
};

channel.BasicConsume(queue:"letterbox", autoAck: false,  eventBasicConsumer);
Console.WriteLine($"Press Enter to Exit");
Console.ReadLine();
