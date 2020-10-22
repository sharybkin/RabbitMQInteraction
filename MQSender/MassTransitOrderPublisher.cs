using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MQCommonObjects.Contracts;
using Newtonsoft.Json;

namespace MQSender
{
    /// <summary>
    /// Публикует сообщение всем(не в конктретную очередь)
    /// </summary>
    public class MassTransitOrderPublisher : IOrderSender
    {
        private readonly IBusControl _busControl;
        public MassTransitOrderPublisher(IBusControl busControl)
        {
            _busControl = busControl;
        }
        public async Task Send(Order order)
        {
            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await _busControl.StartAsync(source.Token);
            try
            {
                await _busControl.Publish<Order>(order);
                Console.WriteLine("Order was published");
                var message = JsonConvert.SerializeObject(order);
                Console.WriteLine(message);
                Console.WriteLine();
            }
            finally
            {
                await _busControl.StopAsync();
            }
        }
    }
}