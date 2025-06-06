using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using TrafficTrackingApi.Controllers;
using TrafficTrackingApi.DataContexts;
using TrafficTrackingApi.Models;

namespace TrafficTrackingTest
{
    public class EventControllerTests
    {
        private readonly Mock<DbSet<Intersection>> _mockIntersections;
        private readonly Mock<DbSet<Event>> _mockEvents;
        private readonly Mock<AppDbContext> _mockContext;
        private readonly EventsController _controller;

        public EventControllerTests()
        {
            _mockIntersections = new Mock<DbSet<Intersection>>();
            _mockEvents = new Mock<DbSet<Event>>();

            _mockContext = new Mock<AppDbContext>();
            _mockContext.Setup(c => c.Intersections).Returns(_mockIntersections.Object);
            _mockContext.Setup(c => c.Events).Returns(_mockEvents.Object);

            _controller = new EventsController(_mockContext.Object);
        }

        [Fact]
        public async Task PostEvent_WithValidData_ReturnsCreatedAtAction()
        {
            var newEvent = new Event
            {
                Type = "Проишествие",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(1),
                TrafficImpactLevel = 3
            };
            int intersectionId = 1;
            var intersection = new Intersection { IntersectionId = intersectionId };

            _mockContext.Setup(c => c.Intersections.FindAsync(intersectionId))
                       .ReturnsAsync(intersection);

            _mockContext.Setup(c => c.Events.Add(newEvent));
            _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(1);

            var result = await _controller.PostEvent(newEvent, intersectionId);

            var actionResult = Assert.IsType<ActionResult<Event>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);

            Assert.Equal("GetEvent", createdAtActionResult.ActionName);
            Assert.Equal(newEvent.EventId, createdAtActionResult.RouteValues["id"]);
            Assert.Equal(newEvent, createdAtActionResult.Value);

            Assert.Contains(intersection, newEvent.Intersections);
        }
    }
}