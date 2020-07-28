using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer
{
    public class Receiver
    {
        public static void Main(string[] args)
        {
            ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };
            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare("BasicTest", false, false, false, null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    //var body = ea.Body.Span;
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine("Received message {0}...", message);

                    //channel.BasicAck(ea.DeliveryTag, false);
                };

                channel.BasicConsume("BasicTest", true, consumer);

                Console.WriteLine("Press [enter] to exit the consumer...");
                Console.ReadLine();
            }
        }
    }
}
