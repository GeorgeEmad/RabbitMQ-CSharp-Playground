using System;
using System.Text;
using System.Collections;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;

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

var eventBasicConsumer = new EventingBasicConsumer(channel);
eventBasicConsumer.Received += (model, eventArguments) =>{
    var body = eventArguments.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"message: {message}");
    var messageDeserialised = JsonConvert.DeserializeObject<List<string>>(message);
};

channel.BasicConsume(queue:"letterbox", autoAck: true,  eventBasicConsumer);
Console.WriteLine($"Press Enter to Exit");
Console.ReadLine();
