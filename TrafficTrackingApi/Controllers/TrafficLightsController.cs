using System;
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
    public class TrafficLightsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TrafficLightsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/TrafficLights
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrafficLight>>> GetTrafficLights()
        {
            return await _context.TrafficLights.ToListAsync();
        }

        // GET: api/TrafficLights/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrafficLight>> GetTrafficLight(int id)
        {
            var trafficLight = await _context.TrafficLights.FindAsync(id);

            if (trafficLight == null)
            {
                return NotFound();
            }

            return trafficLight;
        }

        // PUT: api/TrafficLights/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrafficLight(int id, TrafficLight trafficLight)
        {
            if (id != trafficLight.TrafficLightId)
            {
                return BadRequest();
            }

            _context.Entry(trafficLight).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrafficLightExists(id))
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

        // POST: api/TrafficLights
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TrafficLight>> PostTrafficLight(TrafficLight trafficLight)
        {
            _context.TrafficLights.Add(trafficLight);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTrafficLight", new { id = trafficLight.TrafficLightId }, trafficLight);
        }

        // DELETE: api/TrafficLights/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrafficLight(int id)
        {
            var trafficLight = await _context.TrafficLights.FindAsync(id);
            if (trafficLight == null)
            {
                return NotFound();
            }

            _context.TrafficLights.Remove(trafficLight);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TrafficLightExists(int id)
        {
            return _context.TrafficLights.Any(e => e.TrafficLightId == id);
        }
    }
}
