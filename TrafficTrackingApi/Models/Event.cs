using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TrafficTrackingApi.Models;

public partial class Event
{
    public int EventId { get; set; }

    public string Type { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public string? Description { get; set; }

    public int TrafficImpactLevel { get; set; }

    [JsonIgnore]
    public virtual ICollection<Intersection> Intersections { get; set; } = new List<Intersection>();
}
