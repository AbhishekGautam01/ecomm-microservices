﻿using EventBusRabbitMQ.Interface;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace EventBusRabbitMQ
{
    public class RabbitMQConnection : IRabbitMQConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private IConnection _connection;
        private bool _disposed;
        public RabbitMQConnection(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            if (!isConnected)
                TryConnect();
        }
        public bool isConnected
        {
            get
            {
                return _connection != null && _connection.IsOpen && !_disposed;
            }
        }
        public bool TryConnect()
        {
            try
            {
                _connection = _connectionFactory.CreateConnection();
            }
            catch (BrokerUnreachableException)
            {
                Thread.Sleep(2000);
                _connection = _connectionFactory.CreateConnection();
            }
            return isConnected;
        }
        public IModel CreateModel()
        {
            if (!isConnected)
                throw new InvalidOperationException("No rabbit connection");
            return _connection.CreateModel();
        }

        public void Dispose()
        {
            if (_disposed)
                return;
            try
            {
                _connection.Dispose();
            }
            catch (Exception)
            {
                throw;
            }
        }

        
    }
}
