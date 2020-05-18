using System.Collections.Generic;

namespace Actio.Common.RabbitMQ
{
    public class RabbitMqOptions
    {
        public string ConnectionName { get; set; }
        public IEnumerable<string> HostNames { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public ExchangeOptions Exchange { get; set; }
        public QueueOptions Queue { get; set; }
        public QosOptions Qos { get; set; }

        public class ExchangeOptions
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public bool Declare { get; set; }
            public bool Durable { get; set; }
            public bool AutoDelete { get; set; }
        }
        
        public class QueueOptions
        {
            public bool Declare { get; set; }
            public bool Durable { get; set; }
            public bool Exclusive { get; set; }
            public bool AutoDelete { get; set; }
        }

        public class QosOptions
        {
            public uint PrefetchSize { get; set; }
            public ushort PrefetchCount { get; set; }
            public bool Global { get; set; }
        }
    }
}