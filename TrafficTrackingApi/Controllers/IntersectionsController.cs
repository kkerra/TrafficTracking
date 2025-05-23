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
    public class IntersectionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public IntersectionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Intersections
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Intersection>>> GetIntersections()
        {
            try
            {
                var intersections = await _context.Intersections.ToListAsync();

                if (intersections == null || intersections.Count == 0)
                    return NotFound("Данные не найдены");

                return intersections;
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ошибка сервера");
            }
        }

        // GET: api/Intersections/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Intersection>> GetIntersection(int id)
        {
            var intersection = await _context.Intersections.FindAsync(id);

            if (intersection == null)
            {
                return NotFound();
            }

            return intersection;
        }

        // PUT: api/Intersections/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIntersection(int id, Intersection intersection)
        {
            if (id != intersection.IntersectionId)
            {
                return BadRequest();
            }

            _context.Entry(intersection).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IntersectionExists(id))
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

        // POST: api/Intersections
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Intersection>> PostIntersection(Intersection intersection)
        {
            _context.Intersections.Add(intersection);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIntersection", new { id = intersection.IntersectionId }, intersection);
        }

        // DELETE: api/Intersections/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIntersection(int id)
        {
            var intersection = await _context.Intersections.FindAsync(id);
            if (intersection == null)
            {
                return NotFound();
            }

            _context.Intersections.Remove(intersection);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IntersectionExists(int id)
        {
            return _context.Intersections.Any(e => e.IntersectionId == id);
        }
    }
}
