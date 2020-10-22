using System;
using System.Drawing;
using System.Threading.Tasks;
using MassTransit;
using MQCommonObjects.Contracts;
using Newtonsoft.Json;

namespace MQSender
{
    public class MassTransitOrderSender : IOrderSender
    {
        private readonly IBusControl _busControl;

        public MassTransitOrderSender(IBusControl busControl)
        {
            _busControl = busControl;
        }


        public async Task Send(Order order)
        {
            var endpoint = await _busControl.GetSendEndpoint(new Uri("queue:order-queue"));
            await endpoint.Send(order);
            Console.WriteLine("Order was sent");
            var message = JsonConvert.SerializeObject(order);
            Console.WriteLine(message);
            Console.WriteLine();
        }
    }
}