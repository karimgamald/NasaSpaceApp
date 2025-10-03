using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using SharkTracking.Core.Entities;
using SharkTracking.InfrastructureData.Data;

namespace SharkTracking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SharkController : ControllerBase
    {
        private readonly NasaDbContext _context;
        public SharkController(NasaDbContext context)
        {
            _context = context;
        }

        // GET: api/Sharks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shark>>> GetSharks()
        {
            return await _context.Sharks
                .Include(s => s.SharkTagData)
                .Include(s => s.PredictionAlerts)
                .ToListAsync();
        }
        // GET: api/Sharks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Shark>> GetShark(int id)
        {
            var shark = await _context.Sharks
                .Include(s => s.SharkTagData)
                .Include(s => s.PredictionAlerts)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (shark == null)
            {
                return NotFound();
            }

            return shark;
        }
        // POST: api/Sharks
        [HttpPost]
        public async Task<ActionResult<Shark>> PostShark(Shark shark)
        {
            _context.Sharks.Add(shark);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetShark), new { id = shark.Id }, shark);
        }

        // PUT: api/Sharks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditShark(int id, Shark shark)
        {
            if (id != shark.Id)
            {
                return BadRequest("Shark ID mismatch");
            }

            var existingShark = await _context.Sharks.FindAsync(id);

            if (existingShark == null)
            {
                return NotFound();
            }

            // تحديث الحقول المطلوبة فقط
            existingShark.Name = shark.Name;
            existingShark.Species = shark.Species;
            existingShark.Age = shark.Age;
            existingShark.Length = shark.Length;
            existingShark.Weight = shark.Weight;
            
            await _context.SaveChangesAsync();

            return Ok(existingShark);
        }


        // DELETE: api/Sharks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShark(int id)
        {
            var shark = await _context.Sharks.FindAsync(id);
            if (shark == null)
            {
                return NotFound();
            }

            _context.Sharks.Remove(shark);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
