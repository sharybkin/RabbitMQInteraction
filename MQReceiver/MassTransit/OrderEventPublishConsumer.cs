using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using MQCommonObjects.Contracts;
using Newtonsoft.Json;

namespace MQReceiver.MassTransit
{
    public class OrderEventPublishConsumer : IConsumer<Order>
    {
        private readonly ILogger<OrderEventPublishConsumer> _logger;

        public OrderEventPublishConsumer(ILogger<OrderEventPublishConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<Order> context)
        {
            var order = context.Message;
            _logger.LogInformation("MassTransit Publish Method: Order was received {0}",JsonConvert.SerializeObject(order));
        }
    }
}