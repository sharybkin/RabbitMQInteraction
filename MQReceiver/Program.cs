using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQCommonObjects.Contracts;
using MQReceiver.MassTransit;
using MQReceiver.NativeClient;
using MQReceiver.UsageExamples;
using RabbitMQ.Client;

namespace MQReceiver
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    //общий пример
                    services.AddSingleton(provider => new UsageExamples.NativeClient(args));
                    services.AddHostedService<NativeClientWorker>();

                    //Пример на объекте
                    services.AddSingleton<IReceiver<Order>>(provider => new NativeClientOrderReceiver(
                        new ConnectionFactory {HostName = "localhost", DispatchConsumersAsync = true},
                        provider.GetRequiredService<ILogger<NativeClientOrderReceiver>>()));
                    services.AddHostedService<OrderReceiverWorker>();
                    
                    //Реализация MassTransit
                    services.ConfigureMassTransitReceiver(new Uri("rabbitmq://localhost/MassTransit"));
                });
    }
}