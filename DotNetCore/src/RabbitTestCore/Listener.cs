using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitTestCore
{
    public class Listener : IDisposable
    {
        // EventingBasicConsumer m_consumer;
        private IConnection m_connection;
        private IModel m_channel;
        public Listener(string hostname)
        {
            var factory = new ConnectionFactory() { HostName = hostname };

            factory.UserName = "admin";
            factory.Password = "qwer$asdf!1";

            m_connection = factory.CreateConnection();

            m_channel = m_connection.CreateModel();

            //m_channel.QueueDeclare(queue: "hello",
            //                         durable: false,
            //                         exclusive: false,
            //                         autoDelete: false,
            //                         arguments: null);

            m_channel.ExchangeDeclare(
                exchange: "count",
                type: "fanout",
                durable: true,
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
            m_connection.Dispose();
            m_channel.Dispose();
        }
    }
}
