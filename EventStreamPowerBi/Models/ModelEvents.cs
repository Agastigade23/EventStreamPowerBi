using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventStreamPowerBi.Models
{
    public class ModelEvents
    {
        public int? EventId { get; set; }
        public string? Event { get; set; }
        public string? FeedEventName { get; set; }
        public string? SessionID { get; set; }
    }

    public class StartEvent : ModelEvents
    {
        public string? stationID { get; set; }
    }

    public class UpdateEvent : ModelEvents
    {
        public string? stationTime { get; set; }
        public float? energy { get; set; }

    }
}
