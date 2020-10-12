using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MQReceiver.UsageExamples
{
    public class NativeClient
    {
        private IModel _channel;
        private string _exchangeType;

        public NativeClient(string[] args)
        {
            if (!TryGetMqExchangeType(args, out _exchangeType))
                throw new Exception("Send argument: 'default' or 'direct'");
            
            ConfigureChannel(_channel);
        }

        private void ConfigureChannel(IModel channel)
        {
            var factory = new ConnectionFactory() { HostName = "localhost", DispatchConsumersAsync = true};
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
        }
        public async Task ReceiveMessage()
        {
            if(_exchangeType == "default")
                await ReceiveUsingDefaultExchange(_channel);
            else if(_exchangeType == "direct")
                await ReceiveUsingDirectExchange(_channel);
        }
        
        private async Task ConsumerOnReceived(object? sender, BasicDeliverEventArgs e)
        {
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [x] Received {0}", message);
            await Task.Yield();
            
            _channel.BasicAck(e.DeliveryTag, multiple: false);
        }
        
        private Task ReceiveUsingDefaultExchange(IModel channel)
        {
            channel.QueueDeclare(queue: "test", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += ConsumerOnReceived;
            channel.BasicConsume(queue: "test", autoAck: false, consumer: consumer);
            
            return Task.CompletedTask;
        }

        private Task ReceiveUsingDirectExchange(IModel channel)
        {
            channel.ExchangeDeclare("multiple_receiving", ExchangeType.Direct);

            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queueName, "multiple_receiving", "common");
            
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += ConsumerOnReceived;
            channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
            return Task.CompletedTask;
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
            
            Environment.ExitCode = 1;
            return false;
        }
    }
}