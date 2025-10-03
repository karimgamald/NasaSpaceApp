using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharkTracking.Core.Entities;
using SharkTracking.InfrastructureData.Data;

namespace SharkTracking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SharkTagDataController : ControllerBase
    {
        private readonly NasaDbContext _context;

        public SharkTagDataController(NasaDbContext context)
        {
            _context = context;
        }

        // GET: api/SharkTagData
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SharkTagData>>> GetSharkTagData()
        {
            return await _context.SharkTagData
                .Include(d => d.Shark)
                .ToListAsync();
        }

        // GET: api/SharkTagData/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SharkTagData>> GetSharkTagData(int id)
        {
            var data = await _context.SharkTagData
                .Include(d => d.Shark)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (data == null)
            {
                return NotFound();
            }

            return data;
        }

        //// POST: api/SharkTagData
        //[HttpPost]
        //public async Task<ActionResult<SharkTagData>> PostSharkTagData(SharkTagData tagData)
        //{
        //    // تأكد إن القرش موجود قبل إضافة البيانات
        //    var sharkExists = await _context.Sharks.AnyAsync(s => s.Id == tagData.SharkId);
        //    if (!sharkExists)
        //    {
        //        return BadRequest("Shark with given ID does not exist.");
        //    }

        //    _context.SharkTagData.Add(tagData);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction(nameof(GetSharkTagData), new { id = tagData.Id }, tagData);
        //}

        //// PUT: api/SharkTagData/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> EditSharkTagData(int id, SharkTagData tagData)
        //{
        //    if (id != tagData.Id)
        //    {
        //        return BadRequest("ID mismatch.");
        //    }

        //    var existingData = await _context.SharkTagData.FindAsync(id);
        //    if (existingData == null)
        //    {
        //        return NotFound();
        //    }

        //    // تحديث الحقول
        //    existingData.Timestamp = tagData.Timestamp;
        //    existingData.Latitude = tagData.Latitude;
        //    existingData.Longitude = tagData.Longitude;
        //    existingData.Depth = tagData.Depth;
        //    existingData.WaterTemperature = tagData.WaterTemperature;
        //    existingData.OxygenLevel = tagData.OxygenLevel;
        //    existingData.Chlorophyll = tagData.Chlorophyll;

        //    await _context.SaveChangesAsync();

        //    return Ok(existingData);
        //}

        //// DELETE: api/SharkTagData/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteSharkTagData(int id)
        //{
        //    var data = await _context.SharkTagData.FindAsync(id);
        //    if (data == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.SharkTagData.Remove(data);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}
    }
}
