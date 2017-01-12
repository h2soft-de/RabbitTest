using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitTestCore.NamedQueue
{
    public class Listener : IDisposable
    {
        private IModel m_channel;
        public Listener(IConnection connection)
        {
            m_channel = connection.CreateModel();

            m_channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

            var m_consumer = new EventingBasicConsumer(m_channel);

            m_consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);
            };

            m_channel.BasicConsume(queue: "hello",
                                 noAck: true,
                                 consumer: m_consumer);
        }

        public void Dispose()
        {
            m_channel.Dispose();
        }
    }
}
