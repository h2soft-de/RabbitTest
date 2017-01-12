using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitTestCore.Exchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitTestCore
{
    public class Program
    {        
        static Publisher m_publisher;
        static Subscriber m_subscriber;

        public static void Main(string[] args)
        {            
            Console.WriteLine("Start RabbitMQ test ...");

            IConnection connection;

            if (args.Length == 2)
            {
                string host = args[0];
                connection = ConnectionBuilder.CreateConnection( host, "admin","qwer$asdf!1");
               
                if (args[1] == "pub")
                {
                    Task t = Task.Run(() => { m_publisher = new Publisher(connection); });                    
                }

                if (args[1] == "sub")
                {
                    Task t = Task.Run(() => { m_subscriber = new Subscriber(connection); });                    
                }

                Console.WriteLine("Press [enter] to exit.");
                Console.ReadLine();

                if (m_subscriber != null)
                {
                    m_subscriber.Dispose();
                }

                if (m_publisher != null)
                {
                    m_publisher.Dispose();
                }

                if (connection != null)
                {
                    connection.Dispose();
                }
            }   
            else
            {
                Console.WriteLine("Usage: RabbitTestCore <host> <mode>");
            }                                             
        }
    }
}
