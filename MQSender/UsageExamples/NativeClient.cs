using System;
using System.Text;
using RabbitMQ.Client;

namespace MQSender.UsageExamples
{
    public class NativeClient
    {
        public void SendMessage(string[] args)
        {
            string exchangeType;
            if (!TryGetMqExchangeType(args, out exchangeType))
                return;
            
            var factory = new ConnectionFactory {HostName = "localhost"};
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            var message = exchangeType + " Hello world";

            if(exchangeType == "default")
                SendUsingDefaultExchange(channel, message);
            else if(exchangeType == "direct")
                SendUsingDirectExchange(channel, message);
            

            Console.WriteLine(" [x] Sent {0}", message);
        }
        
        private static void SendUsingDirectExchange(IModel channel, string message)
        {
            channel.ExchangeDeclare("multiple_receiving", ExchangeType.Direct);
            
            var body = Encoding.UTF8.GetBytes(message);
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            
            channel.BasicPublish(exchange: "multiple_receiving", routingKey: "common", basicProperties: properties, body: body);
        }
        
        private static void SendUsingDefaultExchange(IModel channel, string message)
        {
            //durable - очередь не закроется
            channel.QueueDeclare(queue: "test", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: "", routingKey: "test", basicProperties: properties, body: body);
        }

        private static bool TryGetMqExchangeType(string[] args, out string exchangeType)
        {
            exchangeType = "";
            var successfullyValidate = ValidateArguments(args);

            if (successfullyValidate)
                exchangeType = args[0];
            
            return successfullyValidate;
        }

        private static bool ValidateArguments(string[] args)
        {
            if (args.Length != 0 &&
                (args[0] == "default" || args[0] == "direct"))
                return true;
            
            Console.Error.WriteLine("Send argument: 'default' or 'direct'");
            Environment.ExitCode = 1;
            return false;
        }
    }
}