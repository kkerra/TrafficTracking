using System;
using System.Collections.Generic;

namespace TrafficTrackingApi.Models;

public partial class EventLog
{
    public int LogId { get; set; }

    public int EventId { get; set; }

    public string? EventType { get; set; }

    public string LogMessage { get; set; } = null!;

    public DateTime LogTimestamp { get; set; }

    public int? UserId { get; set; }
}
