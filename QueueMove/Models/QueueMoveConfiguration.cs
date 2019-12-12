using System;

namespace QueueMove.Models
{
    public class QueueMoveConfiguration
    {
        public QueueMoveConfiguration()
        {
            
        }

        public string OriginQueueConnectionString { get; set; }

        public string OriginQueueName { get; set; }

        public string DestinationQueueConnectionString { get; set; }

        public string DestinationExchangeName { get; set; }

        public string DestinationQueueName { get; set; }

        public bool StopWhenEmpty { get; set; }

        public static QueueMoveConfiguration Create()
        {
            return new QueueMoveConfiguration
            {
                OriginQueueConnectionString = Environment.GetEnvironmentVariable("OriginQueueConnectionString"),
                OriginQueueName = Environment.GetEnvironmentVariable("OriginQueueName"),
                DestinationQueueConnectionString = Environment.GetEnvironmentVariable("DestinationQueueConnectionString"),
                DestinationExchangeName = Environment.GetEnvironmentVariable("DestinationExchangeName"),
                DestinationQueueName = Environment.GetEnvironmentVariable("DestinationQueueName"),
                StopWhenEmpty = bool.Parse(Environment.GetEnvironmentVariable("StopWhenEmpty"))
            };
        }

        public static QueueMoveConfiguration CreateForDebug()
        {
            return new QueueMoveConfiguration
            {
                OriginQueueConnectionString = "amqp://guest:guest@localhost:5672/vhost-origin",
                OriginQueueName = "origin-queue",
                DestinationQueueConnectionString = "amqp://guest:guest@localhost:5672/vhost-dest",
                DestinationExchangeName = null,//"dest-exchange",
                DestinationQueueName = "dest-queue",
                StopWhenEmpty = true
            };
        }
    }
}
