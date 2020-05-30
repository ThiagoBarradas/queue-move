using RabbitMQ.Client;
using RabbitMQ.Client.Framing;
using System;
using System.Net.Security;
using System.Text;

namespace QueueMove.Queue
{
    public class QueueManager : IDisposable
    {
        private IModel Channel;

        private readonly ChannelFactory ChannelFactory = new ChannelFactory();

        public string ConnectionString { get; set; }
        
        public string ConnectionName { get; set; }

        public string QueueName { get; set; }

        public string ExchangeName { get; set; }

        public QueueManager(string connectionName, string connectionString, string queueName, string exchangeName = null)
        {
            this.ConnectionName = connectionName;
            this.ConnectionString = connectionString;
            this.QueueName = queueName;
            this.ExchangeName = exchangeName;
        }

        public void AddMessage(string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            this.Channel.BasicPublish(
                exchange: this.ExchangeName,
                routingKey: this.QueueName,
                basicProperties: new BasicProperties { Persistent = true }, 
                body: buffer);
        }

        public (ulong, string) GetMessage()
        {
            var message = this.Channel.BasicGet(this.QueueName, false);

            if (message?.Body != null)
            {
                return (message.DeliveryTag, Encoding.UTF8.GetString(message.Body));
            }

            return (0, null);
        }

        public void Ack(ulong deliveryTag)
        {
            this.Channel.BasicAck(deliveryTag, false);
        }

        public void NAck(ulong deliveryTag, bool requeued = true)
        {
            this.Channel.BasicNack(deliveryTag, false, requeued);
        }

        public void TryConnect()
        {
            try
            {
                this.ChannelFactory.CloseConnection();

                var connectionFactory = new ConnectionFactory()
                {
                    RequestedHeartbeat = 30,
                    NetworkRecoveryInterval = TimeSpan.FromSeconds(5), 
                    TopologyRecoveryEnabled = true,
                    Uri = new Uri(this.ConnectionString)
                };

                connectionFactory.Ssl.CertificateValidationCallback += (sender, certificate, chain, errors) => true;
                connectionFactory.Ssl.AcceptablePolicyErrors = SslPolicyErrors.RemoteCertificateChainErrors
                                                             | SslPolicyErrors.RemoteCertificateNameMismatch
                                                             | SslPolicyErrors.RemoteCertificateNotAvailable;

                this.Channel = this.ChannelFactory.Create(connectionFactory);
                Logger.LogLineWithLevel("OK", $"TryConnect: Successfully connected! {this.ConnectionName}");
            }
            catch(Exception e)
            {
                Logger.LogLineWithLevel("ERROR", $"TryConnect: An exception occurred {this.ConnectionName}");
                Logger.LogLineWithLevel("ERROR", "Message: {0}", e.Message);
                throw;
            }
        }

        public void Dispose()
        {
            this.ChannelFactory.CloseConnection();
        }
    }
}
