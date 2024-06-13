using System;
using System.Text;
using System.Collections;
using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Threading.Tasks;

var factory = new ConnectionFactory{
    Uri = new Uri("amqp://guest:guest@localhost:5672"), 
    ClientProvidedName= "Producer App" 
};

using var connectionFactory = factory.CreateConnection();
using var channel = connectionFactory.CreateModel();
channel.QueueDeclare(
    queue:"letterboxtest", 
    durable:false, 
    exclusive:false, 
    autoDelete:false, 
    arguments: null
);

var random = new Random();
var whileIndex = 0;
while(true)
{
    var producingTime = random.Next(1,3);
    var message = new List<string>();
    for(int i = 0; i< 2; i++){
        message.Add($"param {i}");
    }
    var messageSerialised = JsonConvert.SerializeObject(message);
    var encodedMessage = Encoding.UTF8.GetBytes(messageSerialised); 
    Task.Delay(TimeSpan.FromSeconds(producingTime)).Wait();
    channel.BasicPublish("","letterbox", null ,encodedMessage);
    Console.WriteLine($"Number: {whileIndex} Published Serialised JSON Message: {messageSerialised}");
    whileIndex++;
}