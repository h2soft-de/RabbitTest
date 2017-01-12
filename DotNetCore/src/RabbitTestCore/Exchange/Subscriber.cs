using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitTestCore.Exchange
{
    public class Subscriber : IDisposable
    {
        private IModel m_channel;
        public Subscriber(IConnection connection)
        {
            m_channel = connection.CreateModel();

            m_channel.ExchangeDeclare(
                exchange: "count",
                type: "fanout",
                durable: true,
                autoDelete: false,
                arguments: null);

            var queueName = m_channel.QueueDeclare().QueueName;

            m_channel.QueueBind(queue: queueName,
                  exchange: "count",
                  routingKey: "");

            Console.WriteLine(" [*] Waiting for values.");

            var m_consumer = new EventingBasicConsumer(m_channel);
            m_consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);
            };

            m_channel.BasicConsume(queue: queueName,
                                 noAck: true,
                                 consumer: m_consumer);

        }

        public void Dispose()
        {
            m_channel.Dispose();
        }
    }
}
