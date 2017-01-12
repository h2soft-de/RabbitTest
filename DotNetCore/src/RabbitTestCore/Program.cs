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
        public static void Main(string[] args)
        {            
            Console.WriteLine("Start RabbitMQ test ...");

            IConnection connection;
            IDisposable service = null;

            if (args.Length == 2)
            {
                string host = args[0];
                connection = ConnectionBuilder.CreateConnection( host, "admin","qwer$asdf!1");

                Task task;

                switch (args[1])
                {
                    case "pub":
                        task = Task.Run(() => { service = new Publisher(connection); });
                        break;

                    case "sub":
                        task = Task.Run(() => { service = new Subscriber(connection); });
                        break;

                    case "nqs":
                        task = Task.Run(() => { service = new NamedQueue.Sender(connection); });
                        break;

                    case "nql":
                        task = Task.Run(() => { service = new NamedQueue.Listener(connection); });
                        break;

                    default:
                        break;
                }


                Console.WriteLine("Press [enter] to exit.");
                Console.ReadLine();

                if (service != null)
                {
                    service.Dispose();
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
