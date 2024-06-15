﻿using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory{
    Uri = new Uri("amqp://guest:guest@localhost:5672"),
    ClientProvidedName = "Client"
};
var connection = factory.CreateConnection();
var channel = connection.CreateModel();

var replyQueue = channel.QueueDeclare(queue: "", exclusive:true);
channel.QueueDeclare(queue: "request-queue", exclusive: false);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, EventArgs) =>
{
    var message = EventArgs.Body.ToArray();
    var decodedMessage = Encoding.UTF8.GetString(message);
    Console.WriteLine($"Recieved Reply: {decodedMessage}");
};

channel.BasicConsume(queue:replyQueue.QueueName, autoAck: true,  consumer);

var properties = channel.CreateBasicProperties();
properties.CorrelationId = Guid.NewGuid().ToString();
properties.ReplyTo = replyQueue.QueueName;

var requestMessage = "Request message";
var encodedMessage = Encoding.UTF8.GetBytes(requestMessage);
channel.BasicPublish("", routingKey: "request-queue", basicProperties:properties, encodedMessage);
Console.WriteLine($"Request Sent: {requestMessage}");


Console.WriteLine("Enter to Exit");
Console.ReadLine();


