// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

const string queueName = "reservation";
Console.WriteLine("RabbitMQ Application Started!");
//Here we specify the Rabbit MQ Server. we use rabbitmq docker image and use it
var factory = new ConnectionFactory
{
    HostName = "localhost"
};
//Create the RabbitMQ connection using connection factory details as mentioned above
using (var rabbitConnection = factory.CreateConnection())
{
    //Here we create channel with session and model
    using (var channel = rabbitConnection.CreateModel())
    {
        //declare the queue after mentioning name and a few property related to that
        channel.QueueDeclare(queueName, exclusive: false, autoDelete: false);

        //Set Event object which listen message from chanel which is sent by producer
        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (model, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            Console.WriteLine(String.Format("Reservation received: {0}", message));
        };
        //read the message
        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        Console.ReadKey();
    }
}