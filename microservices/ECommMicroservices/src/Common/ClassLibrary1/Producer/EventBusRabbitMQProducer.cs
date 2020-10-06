using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Interface;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace EventBusRabbitMQ.Producer
{
    public class EventBusRabbitMQProducer
    {
        private readonly IRabbitMQConnection _connection;
        public EventBusRabbitMQProducer(IRabbitMQConnection connection)
        {
            _connection = connection;
        }

        public void PublishBasketCheckout(string queueName, BasketCheckout publishModel)
        {
            using(var channel = _connection.CreateModel() )
            {
                channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                string message = JsonConvert.SerializeObject(publishModel);
                byte[] body = Encoding.UTF8.GetBytes(message);

                // to define properties for channel
                IBasicProperties properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.DeliveryMode = 2;

                channel.ConfirmSelect();
                channel.BasicPublish(exchange: "", routingKey: queueName, mandatory: true, basicProperties: properties, body: body);
                channel.WaitForConfirmsOrDie();

                channel.BasicAcks += (sender, eventArgs) =>
                {
                    Console.WriteLine("Sent RabbitMQ");
                    //implement ack handle
                };
                channel.ConfirmSelect();
            }
        }
    }
}
