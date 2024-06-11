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

channel.BasicQos(PrefetchSize:0, PrefetchCount:1, global: false);

var random = new Reandom();
var eventBasicConsumer = new EventingBasicConsumer(channel);
eventBasicConsumer.Received += (model, eventArguments) =>{
    var processingTime = random.Next(1,5);
    var body = eventArguments.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Task.Delay(TimeSpan.FromSeconds(processingTime)).Wait();
    channel.BasicAck(eventArguments.DeliveryTag, multiple: false);
    Console.WriteLine($"message: {message}");
    var messageDeserialised = JsonConvert.DeserializeObject<List<string>>(message);
};

channel.BasicConsume(queue:"letterbox", autoAck: false,  eventBasicConsumer);
Console.WriteLine($"Press Enter to Exit");
Console.ReadLine();
