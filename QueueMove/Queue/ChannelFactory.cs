using RabbitMQ.Client;

namespace QueueMove.Queue
{
    public class ChannelFactory
    {
        private readonly object Lock = new object();

        private IConnection Connection;

        private IModel Model;

        public IModel Create(ConnectionFactory factory)
        {
            if (this.Connection == null)
            {
                lock (this.Lock)
                {
                    if (this.Connection == null)
                    {
                        this.Connection = factory.CreateConnection();
                    }
                }
            }

            if (this.Model == null)
            {
                lock (this.Lock)
                {
                    if (this.Model == null)
                    {
                        this.Model = this.Connection.CreateModel();
                    }
                }
            }

            return this.Model;
        }

        public void CloseConnection()
        {
            if (this.Connection != null)
            {
                lock (this.Lock)
                {
                    if (this.Connection != null)
                    {
                        this.Connection.Close();
                        this.Connection.Dispose();
                        this.Connection = null;
                    }
                }
            }
        }
    }
}
