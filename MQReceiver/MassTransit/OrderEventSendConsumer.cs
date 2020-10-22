using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using MQCommonObjects.Contracts;
using Newtonsoft.Json;

namespace MQReceiver.MassTransit
{
    public class OrderEventSendConsumer : IConsumer<Order>
    {
        private readonly ILogger<OrderEventSendConsumer> _logger;

        public OrderEventSendConsumer(ILogger<OrderEventSendConsumer> logger)
        {
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<Order> context)
        {
            var order = context.Message;
            _logger.LogInformation("MassTransit Send Method: Order was received {0}",JsonConvert.SerializeObject(order));
        }
    }
}