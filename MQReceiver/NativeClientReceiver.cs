using System;
using System.Drawing;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MQCommonObjects.Dtos;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MQReceiver
{
    public class NativeClientOrderReceiver : IReceiver<Order>
    {
        private readonly IModel _channel;
        private readonly ILogger<NativeClientOrderReceiver> _logger;
        private readonly string _queueName;

        public NativeClientOrderReceiver(IConnectionFactory factory, ILogger<NativeClientOrderReceiver> logger)
        {
            _logger = logger;
            _channel = GetChannel(factory);
            _queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(_queueName, "multiple_receiving_orders", "simple_order");
        }

        public Task Receive()
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += ConsumerOnReceived;
            _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);
            return Task.CompletedTask;
        }
        
        private IModel GetChannel(IConnectionFactory factory)
        {
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            
            channel.ExchangeDeclare("multiple_receiving_orders", ExchangeType.Direct);

            return channel;
        }
        
        private async Task ConsumerOnReceived(object? sender, BasicDeliverEventArgs e)
        {
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var order = JsonConvert.DeserializeObject<Order>(message);
            _logger.LogInformation("Order was received {0}",JsonConvert.SerializeObject(order));
            await Task.Yield();
            
            _channel.BasicAck(e.DeliveryTag, multiple: false);
        }
    }
}