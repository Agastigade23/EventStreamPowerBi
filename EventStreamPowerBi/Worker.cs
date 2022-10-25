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
        private static EventHubClient eventHubClient;
        private const string EventHubConnectionString = "Endpoint=sb://azureeventhubdemotest.servicebus.windows.net/;SharedAccessKeyName=EventHubdemoSAS;SharedAccessKey=6y7emH3yDPqmfj8jy/GVXZQZSxjFFyItKjtTClbIqPM=;EntityPath=eventhubdemo";
        private const string EventHubName = "eventhubdemo";
        private static int i = 0;
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            _timer = new Timer(ProduceChargeEvent, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(10));

            return Task.CompletedTask;
        }

        public async void ProduceChargeEvent(object? state)
        {
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(EventHubConnectionString)
            {
                EntityPath = EventHubName
            };

            eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());

            if(i==0)
                i = 1;
            else
                i++;
            await SendMessagesToEventHub(i);

            await eventHubClient.CloseAsync();
        }

        public class WeatherForecast
        {
            public int messageId { get; set; }
            public string? deviceId { get; set; }
            public float? temperature { get; set; }
            public float? humidity { get; set; }
        }

        private static async Task SendMessagesToEventHub(int numMessagesToSend)
        {
            
            var random = new Random();
            int randomnumber = random.Next(1, 50);
            var random1 = new Random();
            int randomnumber1 = random1.Next(1, 50);
            try
                {
                    var weatherForecast = new WeatherForecast
                    {
                        messageId = i,
                        deviceId = Guid.NewGuid().ToString(),
                        temperature = (float)randomnumber,
                        humidity = (float)randomnumber1
                    };
                    string jsonString = JsonSerializer.Serialize(weatherForecast);
                    //var message = $"Message {i}";

                    Console.WriteLine($"Sending message: {jsonString}");
                    await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(jsonString)));
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
                }

                await Task.Delay(10);

            Console.WriteLine($"{numMessagesToSend} messages sent.");
        }
    }
}