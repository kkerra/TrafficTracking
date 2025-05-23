using System;
using System.Collections.Generic;

namespace TrafficTrackingApi.Models;

public partial class TrafficLight
{
    public int TrafficLightId { get; set; }

    public int IntersectionId { get; set; }

    public string Type { get; set; } = null!;

    public string State { get; set; } = null!;

    public DateOnly? InstallationDate { get; set; }

    public virtual Intersection Intersection { get; set; } = null!;
}
