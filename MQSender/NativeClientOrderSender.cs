using System;
using System.Text;
using MQCommonObjects.Dtos;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace MQSender
{
    public class NativeClientOrderSender : IOrderSender
    {
        private readonly IConnectionFactory _factory;
        
        public NativeClientOrderSender(IConnectionFactory connectionFactory)
        {
            _factory = connectionFactory;
        }
        
        public void Send(Order order)
        {
            using var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();
            
            channel.ExchangeDeclare("multiple_receiving_orders", ExchangeType.Direct);

            var message = JsonConvert.SerializeObject(order);
            var body = Encoding.UTF8.GetBytes(message);
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            
            channel.BasicPublish(exchange: "multiple_receiving_orders", routingKey: "simple_order", basicProperties: properties, body: body);

            Console.WriteLine("Order was sent");
            Console.WriteLine(message);
            Console.WriteLine();
        }
    }
}