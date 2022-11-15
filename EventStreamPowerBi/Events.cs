using Microsoft.Azure.EventHubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EventStreamPowerBi.Models;


namespace EventStreamPowerBi
{
    public class Events : IEvents
    {
        private static EventHubClient? eventHubClient;
        private static int i = 0;
        public void ProduceChargeEvent(string EventHubConnectionString, string EventHubName)
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
            _ = SendMessagesToEventHub(i);

            
        }

        private static Task SendMessagesToEventHub(int numMessagesToSend)
        {
            try
            {
                //_ = StartSendMessage(numMessagesToSend);
                _ = UpdateSendMessage(numMessagesToSend);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
            }

            return Task.CompletedTask;
        }

        private static Task StartSendMessage(int numMessagesToSend)
        {
            return StartSendMessage(
                numMessagesToSend,
                eventHubClient);
        }

        private static Task UpdateSendMessage(int numMessagesToSend)
        {
            return UpdateSendMessage(numMessagesToSend, eventHubClient);
        }

        private static async Task StartSendMessage(int numMessagesToSend, EventHubClient eventHubClient)
        {
            try
            {
                var StartEventsobj = new StartEvent
                {
                    Event = "Start - "+ numMessagesToSend ,
                    FeedEventName = "A",
                    stationID = "123",
                    SessionID = Guid.NewGuid().ToString()
                };
                string jsonString = JsonSerializer.Serialize(StartEventsobj);
                //var message = $"Message {i}";

                Console.WriteLine($"Sending message: {jsonString}");
                await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(jsonString)));
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
            }

            await Task.Delay(2);

            Console.WriteLine($"{numMessagesToSend} messages sent.");
        }

        private static async Task UpdateSendMessage(int numMessagesToSend, EventHubClient eventHubClient)
        {
            Random rnd = new();
            int randomnumber = rnd.Next(1, 50);
            try
            {
                var UpdateEventsobj = new UpdateEvent
                {
                    EventId= numMessagesToSend,
                    Event = "Update - " + numMessagesToSend,
                    FeedEventName = "A",
                    stationTime = DateTime.UtcNow.ToString(),
                    SessionID = Guid.NewGuid().ToString(),
                };

                for (int i = 0; i < randomnumber; i++)
                {
                    UpdateEventsobj.energy=(float)i;
                }
                string jsonString = JsonSerializer.Serialize(UpdateEventsobj);
                //var message = $"Message {i}";

                Console.WriteLine($"Sending message: {jsonString}");
                await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(jsonString)));
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
            }

            await Task.Delay(2);

            Console.WriteLine($"{numMessagesToSend} messages sent.");
        }

        private static async Task StopSendMessage(int numMessagesToSend, EventHubClient eventHubClient)
        {
            try
            {
                var StartEventsobj = new StartEvent
                {
                    Event = "Start",
                    FeedEventName = "a",
                    stationID = "123",
                    SessionID = Guid.NewGuid().ToString()
                };
                string jsonString = JsonSerializer.Serialize(StartEventsobj);
                //var message = $"Message {i}";

                Console.WriteLine($"Sending message: {jsonString}");
                await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(jsonString)));
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
            }

            await Task.Delay(2);

            Console.WriteLine($"{numMessagesToSend} messages sent.");
        }
    }
}
