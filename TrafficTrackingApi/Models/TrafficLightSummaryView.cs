using System;
using System.Collections.Generic;

namespace TrafficTrackingApi.Models;

public partial class TrafficLightSummaryView
{
    public int TrafficLightId { get; set; }

    public string TrafficLightType { get; set; } = null!;

    public string TrafficLightState { get; set; } = null!;

    public DateOnly? InstallationDate { get; set; }

    public int IntersectionId { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public int LanesAmount { get; set; }

    public bool IsTurningLanes { get; set; }

    public DateOnly? UpdateDate { get; set; }
}
