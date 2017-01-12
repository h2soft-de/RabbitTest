using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;
using System.Text;
using System.Threading;

namespace RabbitTestCore.NamedQueue

{
    public class Sender : IDisposable
    {
        private Timer m_timer;
        private IModel m_channel;

        private int m_counter;

        public Sender(IConnection connection)
        {
            Console.WriteLine("RabbitMQ sender init");

            m_channel = connection.CreateModel();

            m_channel.QueueDeclare(queue: "hello",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);


            m_counter = 0;
            m_timer = new Timer(Send, null, 2000, 2000);
        }

        public void Dispose()
        {
            m_timer.Dispose();
            m_channel.Dispose();
        }

        private void Send(object state)
        {
            string message = $"Value: { m_counter++ }";
            var body = Encoding.UTF8.GetBytes(message);

            m_channel.BasicPublish(exchange: "",
                                 routingKey: "hello",
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine(" [x] Sent {0}", message);
        }
    }
}
