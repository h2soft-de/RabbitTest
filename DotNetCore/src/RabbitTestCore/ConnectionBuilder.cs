using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitTestCore
{
    public static class ConnectionBuilder
    {
        public static IConnection CreateConnection(string host, string username, string password)
        {
            var factory = new ConnectionFactory();

            factory.HostName = host;

            factory.UserName = username;
            factory.Password = password;

            return factory.CreateConnection();
        }
    }
}
