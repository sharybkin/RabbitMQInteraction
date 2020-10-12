using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MQReceiver.UsageExamples
{
    public class NativeClientWorker : BackgroundService
    {
        private readonly NativeClient _client;
        public NativeClientWorker(NativeClient client)
        {
            _client = client;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)  
        {  
            stoppingToken.ThrowIfCancellationRequested();
            await _client.ReceiveMessage();
        }
    }
}