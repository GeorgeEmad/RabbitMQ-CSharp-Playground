using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading.Tasks;

var connectionFactory = new ConnectionFactory{
    Uri = new Uri("amqp:guest:quest@localhost:5672"),
    ClientProvidedName = "Producer App" 
};

var connecion = connectionFactory.CreateConnection();
var channel = connecion.CreateModel();
channel.QueueDeclare(
    queue: "letterbox2",
    durable: false,
    exclusive: false,
    autoDelete:false,
    arguments: null
);
channel.BasicQos(0, 1,  false);
channel.ExchangeDeclare(exchange: "FanOutExchange", type: ExchangeType.Fanout);
channel.QueueBind("letterbox2", "FanOutExchange", "");

var random = new Random();
var coonsumer = new EventingBasicConsumer(channel);
coonsumer.Received += (model, eventArguments) =>{
    var processingTime = random.Next(1,5);
    var body = eventArguments.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Task.Delay(TimeSpan.FromSeconds(processingTime)).Wait();
    channel.BasicAck(eventArguments.DeliveryTag, multiple: false);
    Console.WriteLine($"Consumer1: message: {message} was processed in {processingTime}s");
};

channel.BasicConsume(queue:"letterbox2", autoAck: false,  coonsumer);
Console.WriteLine($"Press Enter to Exit");
Console.ReadLine();
