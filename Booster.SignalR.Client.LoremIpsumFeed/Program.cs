using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Booster.Configuration;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Booster.SignalR.Client.LoremIpsumFeed
{
   
    public static class Program
    {
        private static int signalRClientsToStart = 1;
        private static int bufferSize = 100;

        static async Task Main()
        {
            string rootDirectory = Directory.GetCurrentDirectory();

            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(rootDirectory)
                .SetupAppSettingsAndLogging()
                .Build();
            ILogger logger = config.CreateSerilogLogger();

            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            var inputString = "";
            int inputVal;

            Console.WriteLine("How many Lorem Ipsum streams should we start?");

            while (!int.TryParse(inputString, out inputVal))
            {
                Console.Write("Please enter a valid number and hit Enter: ");
                inputString = Console.ReadLine();
            }

            signalRClientsToStart = inputVal;
            
            logger.LogInformation($"Starting {signalRClientsToStart} SignalR clients. Each client will stream a portion of Lorem Ipsum stream to the server");


            Parallel.For(0, signalRClientsToStart, async (index) => await StreamLoremIpsum(index, logger, config["AppSettings:SignalRHubUrl"]));

            Console.WriteLine("Press any key to stop streaming");
            Console.ReadKey();
        }

        private static async Task StreamLoremIpsum(int index, ILogger logger, string signalRUrl)
        {
            index++;
            var connection = new HubConnectionBuilder()
                .WithUrl(signalRUrl)
                .Build();

            async Task OnConnectionOnClosed(Exception error)
            {
                logger.LogError(error, "Hub connection error.");
                logger.LogWarning($"Client #{index}. connection was closed. Restarting...");
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            }

            connection.Closed += OnConnectionOnClosed;

            await connection.StartAsync();
            logger.LogInformation($"Client #{index}. connection opened...");

            async IAsyncEnumerable<string> ClientStreamData()
            {
                using (var loremIpsumStream = new DevTest.LorumIpsumStream())
                {
                    using (var streamReader = new StreamReader(loremIpsumStream, Encoding.Unicode, true))
                    {
                        string text = null, leftOvers = null;
                        do
                        {
                            var data = await ReadUntilNextSpace(streamReader);
                            if (!string.IsNullOrWhiteSpace(data.text))
                            {
                                // it would be easier if source stream allowed seeking, but we can workaround that issue. 
                                text = leftOvers + data.text;
                                leftOvers = data.leftOvers; //going to be appended 
                                logger.LogDebug(
                                    $"Client {index}. Fetched data from the stream: {text}. Sending to the server");
                                yield return text;
                                await Task.Delay(1000);
                            }

                        } while (!string.IsNullOrWhiteSpace(text));

                        if (!string.IsNullOrWhiteSpace(leftOvers))
                        {
                            yield return leftOvers;
                        }
                    }
                }
            }

            await connection.SendAsync("UploadLoremIpsumStream", ClientStreamData());

        }

        public static async Task<(string text, string leftOvers)> ReadUntilNextSpace(this StreamReader sr)
        {

            string leftOvers = string.Empty;
            StringBuilder sb = new StringBuilder();
            bool found = false;

            while (!found && !sr.EndOfStream)
            {
                var buffer = new char[bufferSize];
                int count = await sr.ReadAsync(buffer, 0, bufferSize);

                for (int i = 0; i < count; i++)
                {
                    if (char.IsWhiteSpace(buffer[i]))
                    {
                        found = true;
                        var upperBound = i + 1;
                        sb.Append(buffer[..upperBound]);
                        leftOvers = new string(buffer[upperBound..]);
                        break;
                    }
                }

                if (!found)
                {
                    sb.Append(buffer[..count]);
                }

                
            }

            return (text: sb.ToString(), leftOvers: leftOvers);
        }

    }


}
