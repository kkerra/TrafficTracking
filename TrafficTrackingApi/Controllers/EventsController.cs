﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrafficTrackingApi.DataContexts;
using TrafficTrackingApi.Models;

namespace TrafficTrackingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EventsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Events/intersection/{intersectionId}
        [HttpGet("intersection/{intersectionId}")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventsByIntersection(int intersectionId)
        {
            var events = await _context.Events
                .Where(e => e.Intersections.Any(i => i.IntersectionId == intersectionId))
                .ToListAsync();

            if (events == null || !events.Any())
            {
                return NotFound();
            }

            return Ok(events);
        }


        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var @event = await _context.Events.FindAsync(id);

            if (@event == null)
            {
                return NotFound();
            }

            return @event;
        }

        // PUT: api/Events/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, Event @event)
        {
            if (id != @event.EventId)
            {
                return BadRequest();
            }

            _context.Entry(@event).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent(Event newEvent, [FromQuery] int intersectionId)
        {
            try
            {
                if (newEvent == null)
                {
                    return BadRequest("Событие необходимо заполнить.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var intersection = await _context.Intersections.FindAsync(intersectionId);
                if (intersection == null)
                {
                    return NotFound("Перекресток не найден.");
                }

                newEvent.Intersections.Add(intersection);

                _context.Events.Add(newEvent);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetEvent", new { id = newEvent.EventId }, newEvent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ошибка сервера.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.EventId == id);
        }
    }
}
