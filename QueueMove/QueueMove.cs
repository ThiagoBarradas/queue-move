using QueueMove.Models;
using QueueMove.Queue;
using System;
using System.Threading;

namespace QueueMove
{
    public class QueueMessageProcessor : IDisposable
    {
        public QueueManager OriginQueueManager { get; set; }

        public QueueManager DestinationQueueManager { get; set; }

        public QueueMoveConfiguration Configuration { get; set; }

        public QueueMessageProcessor(QueueMoveConfiguration configuration)
        {
            this.Configuration = configuration;
            
            this.OriginQueueManager = new QueueManager("Origin", 
                configuration.OriginQueueConnectionString, 
                configuration.OriginQueueName, 
                null ?? "");

            this.DestinationQueueManager = new QueueManager("Destination", 
                configuration.DestinationQueueConnectionString, 
                configuration.DestinationQueueName, 
                configuration.DestinationExchangeName ?? "");

            this.OriginQueueManager.TryConnect();
            this.DestinationQueueManager.TryConnect();
        }

        public bool Execute()
        {
            try
            {
                var (deliveryTag, message) = this.OriginQueueManager.GetMessage();
               
                if (message != null)
                {
                    Logger.LogLineWithLevel("OK", "HandleReceivedMessage: Processing message [{0}] started", deliveryTag);

                    try
                    {
                        this.DestinationQueueManager.AddMessage(message);
                        this.OriginQueueManager.Ack(deliveryTag);
                    }
                    catch (Exception)
                    {
                        this.OriginQueueManager.NAck(deliveryTag);
                    }
                }
                else if (this.Configuration.StopWhenEmpty)
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Logger.LogLineWithLevel("ERROR", "Execute: An exception occurred");
                Logger.LogLineWithLevel("ERROR", "Message: {0}", e.Message);
                Thread.Sleep(500);
            }

            return true;
        }

        public void Dispose()
        {
            this.OriginQueueManager.Dispose();
            this.DestinationQueueManager.Dispose();
            Logger.LogLineWithLevel("INFO","Queue Consumer Application Finish");
        }
    }
}