using System;
using ClienteAPI.Models;

namespace ClienteAPI.Models
{
    public class RabbitMQMessage
    {
        public DateTime Timestamp { get; set; }
        public string ActionType { get; set; }
        public string Details { get; set; }
        public object Data { get; set; }

        public RabbitMQMessage(string actionType, string details, object data)
        {
            Timestamp = DateTime.UtcNow;
            ActionType = actionType;
            Details = details;
            Data = data;
        }
    }
}