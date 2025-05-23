using System;
using System.Collections.Generic;

namespace TrafficTrackingApi.Models;

public partial class Intersection
{
    public int IntersectionId { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public int LanesAmount { get; set; }

    public bool IsTurningLanes { get; set; }

    public DateOnly? UpdateDate { get; set; }

    public virtual ICollection<TrafficLight> TrafficLights { get; set; } = new List<TrafficLight>();

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
