using System;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace MQReceiver.MassTransit
{
    public static class MassTransitExtension
    {
        public static IServiceCollection ConfigureMassTransitReceiver(this IServiceCollection services, Uri uri)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<OrderEventPublishConsumer>();
                x.AddConsumer<OrderEventSendConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(uri);
                    cfg.ConfigureEndpoints(context);
                    cfg.ReceiveEndpoint("order-queue", conf =>
                    {
                        conf.Consumer<OrderEventSendConsumer>(context);
                    });
                });
            });

            services.AddMassTransitHostedService();

            return services;
        }
    }
}