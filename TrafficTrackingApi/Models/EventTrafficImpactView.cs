using System;
using System.Collections.Generic;

namespace TrafficTrackingApi.Models;

public partial class EventTrafficImpactView
{
    public int EventId { get; set; }

    public string EventType { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public string? Description { get; set; }

    public int TrafficImpactLevel { get; set; }

    public int? EventDurationMinutes { get; set; }
}
