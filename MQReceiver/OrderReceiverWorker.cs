using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MQCommonObjects.Dtos;

namespace MQReceiver
{
    public class OrderReceiverWorker: BackgroundService
    {
        private readonly IReceiver<Order> _orderReceiver;

        public OrderReceiverWorker(IReceiver<Order> orderReceiver)
        {
            _orderReceiver = orderReceiver;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)  
        {  
            stoppingToken.ThrowIfCancellationRequested();
            await _orderReceiver.Receive();
        }
    }
}