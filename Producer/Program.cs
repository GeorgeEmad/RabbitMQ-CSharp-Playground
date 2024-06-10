// See https://aka.ms/new-console-template for more information
using System;
using System.Text;
using System.Collections;
using RabbitMQ.Client;
using Newtonsoft.Json;

var factory = new ConnectionFactory{
    Uri = new Uri("amqp://guest:guest@localhost:5672"), 
    ClientProvidedName= "Producer App" 
};

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
channel.QueueDeclare(
    queue:"letterboxtest", 
    durable:false, 
    exclusive:false, 
    autoDelete:false, 
    arguments: null
);
var message = new List<string>();
for(int i = 0; i< 1000; i++){
    message.Add($"{i}");
}
var messageSerialised = JsonConvert.SerializeObject(message);
var encodedMessage = Encoding.UTF8.GetBytes(messageSerialised); 
channel.BasicPublish("","letterbox", null ,encodedMessage);
Console.WriteLine($"Published Serialised Message: {messageSerialised}");