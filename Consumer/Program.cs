using System;
using System.Text;
using System.Collections;
using RabbitMQ.Client;


var connectionFactory = new ConnectionFactory(
    Uri = new Uri("amqp:guest:quest@localhost:5672"),
    ClientProvidedName = "Producer App" 
)
