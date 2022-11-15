using System.Text.Json;
using System.Text;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Extensions.Configuration;

namespace EventStreamPowerBi
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private Timer? _timer = null;
        private readonly IEvents _producer;
        private IConfiguration Configuration;

        public Worker(ILogger<Worker> logger, IEvents producer, IConfiguration _configuration)
        {
            _logger = logger;
            _producer = producer;
            Configuration = _configuration;
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
            string EventHubConnectionString = this.Configuration.GetSection("ConnectionStrings")["EventHubConfigConnectionString"];
            string EventHubName = this.Configuration.GetSection("ConnectionStrings")["EventHubName"];
            _producer.ProduceChargeEvent(EventHubConnectionString, EventHubName);
        }


    }
}