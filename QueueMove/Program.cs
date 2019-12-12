using QueueMove.Models;
using System;
using System.Text.RegularExpressions;

namespace QueueMove
{
    public class Program
    {
        public static void Main(string[] args = null)
        {
            try
            {
                var config = QueueMoveConfiguration.CreateForDebug();
                DisplayHeader(config);

                var processor = new QueueMessageProcessor(config);

                while (processor.Execute()) { }

                processor.OriginQueueManager.Dispose();
                processor.DestinationQueueManager.Dispose();

                Console.WriteLine("Origin queue is empty :)");
            }
            catch(Exception e)
            {
                Console.WriteLine("Program Exception:");
                Console.WriteLine(" - {0}\n\n{1}", e.Message, e.StackTrace);
            }
        }

        private static void DisplayHeader(QueueMoveConfiguration config)
        {
            Logger.LogLineWithLevel("INFO", "Queue Move Application Started");
            Logger.LogLine("");
            Logger.LogLine("Configuration:");
            Logger.LogLine("- OriginQueueConnectionString: {0}", Regex.Replace(config.OriginQueueConnectionString, "(\\:\\/\\/).*(\\@)", "://*****@"));
            Logger.LogLine("- OriginQueueName: {0}", config.OriginQueueName);
            Logger.LogLine("- DestinationQueueConnectionString: {0}", Regex.Replace(config.DestinationQueueConnectionString, "(\\:\\/\\/).*(\\@)", "://*****@"));
            Logger.LogLine("- DestinationExchangeName: {0}", config.DestinationExchangeName);
            Logger.LogLine("- DestinationQueueName: {0}", config.DestinationQueueName);
            Logger.LogLine("- StopWhenEmpty: {0}", config.StopWhenEmpty);
            Logger.LogLine("");
        }
    }

    public static class Logger
    {
        public static void LogLineWithLevel(string logLevel, string message, params object[] args)
        {
            var finalMessage = $"[{GetCurrentDate()}][{logLevel}] {message ?? ""}";
            Console.WriteLine(finalMessage, args);
        }

        public static void LogWithLevel(string logLevel, string message, params object[] args)
        {
            var finalMessage = $"[{GetCurrentDate()}][{logLevel}] {message ?? ""}";
            Console.Write(finalMessage, args);
        }

        public static void LogLine(string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }

        public static void Log(string message, params object[] args)
        {
            Console.Write(message, args);
        }

        private static string GetCurrentDate()
        {
            return DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
