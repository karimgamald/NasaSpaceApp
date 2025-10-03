using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharkTracking.Core.Entities;
using SharkTracking.InfrastructureData.Data;

namespace SharkTrackingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SatelliteDataController : ControllerBase
    {
        private readonly NasaDbContext _context;

        public SatelliteDataController(NasaDbContext context)
        {
            _context = context;
        }

        // GET: api/SatelliteData
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SatelliteData>>> GetSatelliteData()
        {
            return await _context.SatelliteData.ToListAsync();
        }

        // GET: api/SatelliteData/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SatelliteData>> GetSatelliteData(int id)
        {
            var data = await _context.SatelliteData.FindAsync(id);

            if (data == null)
            {
                return NotFound();
            }

            return data;
        }

        // POST: api/SatelliteData
        [HttpPost]
        public async Task<ActionResult<SatelliteData>> PostSatelliteData(SatelliteData satelliteData)
        {
            _context.SatelliteData.Add(satelliteData);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSatelliteData), new { id = satelliteData.Id }, satelliteData);
        }

        // PUT: api/SatelliteData/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSatelliteData(int id, SatelliteData satelliteData)
        {
            if (id != satelliteData.Id)
            {
                return BadRequest("ID mismatch.");
            }

            var existingData = await _context.SatelliteData.FindAsync(id);
            if (existingData == null)
            {
                return NotFound();
            }

            // تحديث القيم
            existingData.Timestamp = satelliteData.Timestamp;
            existingData.Latitude = satelliteData.Latitude;
            existingData.Longitude = satelliteData.Longitude;
            existingData.SeaSurfaceTemperature = satelliteData.SeaSurfaceTemperature;
            existingData.OxygenLevel = satelliteData.OxygenLevel;
            existingData.ChlorophyllConcentration = satelliteData.ChlorophyllConcentration;
            existingData.LightLevel = satelliteData.LightLevel;
            existingData.TideLevel = satelliteData.TideLevel;

            await _context.SaveChangesAsync();

            return Ok(existingData);
        }

        // DELETE: api/SatelliteData/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSatelliteData(int id)
        {
            var data = await _context.SatelliteData.FindAsync(id);
            if (data == null)
            {
                return NotFound();
            }

            _context.SatelliteData.Remove(data);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
