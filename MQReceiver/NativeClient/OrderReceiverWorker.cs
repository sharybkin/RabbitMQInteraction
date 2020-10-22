using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MQCommonObjects.Contracts;

namespace MQReceiver.NativeClient
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