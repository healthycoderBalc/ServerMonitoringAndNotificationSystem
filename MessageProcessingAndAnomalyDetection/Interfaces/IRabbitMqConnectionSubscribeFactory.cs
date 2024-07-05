using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageProcessingAndAnomalyDetection.Interfaces
{
    public interface IRabbitMqConnectionSubscribeFactory
    {
        IConnection CreateConnection();
        IModel CreateChannel(IConnection connection);
    }
}
