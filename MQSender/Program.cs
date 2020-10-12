using System;
using System.Text;
using System.Threading.Tasks;
using MQCommonObjects.Dtos;
using MQSender.UsageExamples;
using RabbitMQ.Client;

namespace MQSender
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var orderId = 1;
            while (true)
            {
                //Пример отправки текстового сообщения
                new NativeClient().SendMessage(args);
                
                
                var order = new Order(orderId, $"детализация заказа №{orderId}");
                IOrderSender sender = new NativeClientOrderSender(new ConnectionFactory {HostName = "localhost"});
                
                sender.Send(order);
                
                await Task.Delay(5000);
            }
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}