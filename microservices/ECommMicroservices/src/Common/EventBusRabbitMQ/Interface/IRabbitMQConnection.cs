using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBusRabbitMQ.Interface
{
    public interface IRabbitMQConnection: IDisposable
    {
        bool isConnected { get; }
        bool TryConnect();
        IModel CreateModel();
    }
}
