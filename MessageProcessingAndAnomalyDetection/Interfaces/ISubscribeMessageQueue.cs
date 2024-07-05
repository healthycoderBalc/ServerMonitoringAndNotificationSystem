using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageProcessingAndAnomalyDetection.Interfaces
{
    public interface ISubscribeMessageQueue
    {
        void Subscribe(string topic, Action<string, string> handleMessage);
    }
}
