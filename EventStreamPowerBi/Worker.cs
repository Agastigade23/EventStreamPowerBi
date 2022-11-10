using System.Text.Json;
using System.Text;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.Amqp.Framing;

namespace EventStreamPowerBi
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private Timer? _timer = null;
        private readonly IEvents _producer;
        
        public Worker(ILogger<Worker> logger, IEvents producer)
        {
            _logger = logger;
            _producer = producer;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            _timer = new Timer(ProduceChargeEvent, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(10));

            return Task.CompletedTask;
        }

        public void ProduceChargeEvent(object? state)
        {
            _producer.ProduceChargeEvent();
        }


    }
}