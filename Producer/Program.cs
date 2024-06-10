// See https://aka.ms/new-console-template for more information
using System;
using System.Text;
using System.Collections;
using RabbitMQ.Client;

var factory = new ConnectionFactory{
    Uri = new Uri("amqp://guest:guest@localhost:5672"), 
    ClientProvidedName= "Producer App" 
};

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
channel.QueueDeclare(
    queue:"letterbox", 
    durable:false, 
    exclusive:false, 
    autoDelete:false, 
    arguments: null
);
for(int i = 0; i< 1000; i++){
    var messgae = $"Test RabbitMQ {i}";
    var encodedMessage = Encoding.UTF8.GetBytes(messgae); 
    channel.BasicPublish("","letterbox", null ,encodedMessage);
    Console.WriteLine($"Published: {messgae}");
}