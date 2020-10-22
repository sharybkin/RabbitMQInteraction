using System;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MQCommonObjects.Contracts;
using MQSender.UsageExamples;
using RabbitMQ.Client;

namespace MQSender
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var orderId = 1;
            IBusControl busControl = Bus.Factory.CreateUsingRabbitMq(x =>
            {
                x.Host(new Uri("rabbitmq://localhost/MassTransit"));
            });
            while (true)
            {
                //Пример отправки текстового сообщения
                new NativeClient().SendMessage(args);
                
                var order = new Order(orderId, $"детализация заказа №{orderId}");
                //Отправка объекта через нативный клиент
                //IOrderSender sender = new NativeClientOrderSender(new ConnectionFactory {HostName = "localhost"});
                
                //Отправка объекта через нативный клиент методом Publish
                //IOrderSender sender = new MassTransitOrderPublisher(busControl);
                
                //Отправка объекта через нативный клиент методом Send
                IOrderSender sender = new MassTransitOrderSender(busControl);

                await sender.Send(order);
                
                await Task.Delay(5000);
            }
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}