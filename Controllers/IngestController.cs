using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharkTracking.Core.Entities;
using SharkTracking.InfrastructureData.Data;
using System;

namespace SharkTracking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //    جهاز التتبع اللي على القرش يبعت Location + Depth كل دقيقة → الكنترولر يستقبل ويحفظ.

    //بيانات القمر الصناعي MODIS عن درجة الحرارة كل 6 ساعات → الكنترولر يستقبل ويحفظ.

    //بعد كده الباحث/الفريق يستخدم البيانات دي في الـ Dashboard و Heatmaps اللي إنت بنيتها.
    public class IngestController : ControllerBase
    {
        private readonly NasaDbContext _context;
        public IngestController(NasaDbContext context)
        {
            _context = context;
        }

        [HttpPost("sharktag")]
        public async Task<IActionResult> IngestSharkTag([FromBody] SharkTagData model)
        {
            model.Timestamp = model.Timestamp == default ? DateTime.UtcNow : model.Timestamp;
            _context.SharkTagData.Add(model);
            await _context.SaveChangesAsync();
            return CreatedAtAction(null, new { id = model.Id }, model);
        }

        [HttpPost("satellite")]
        public async Task<IActionResult> IngestSatellite([FromBody] SatelliteData model)
        {
            model.Timestamp = model.Timestamp == default ? DateTime.UtcNow : model.Timestamp;
            _context.SatelliteData.Add(model);
            await _context.SaveChangesAsync();
            return CreatedAtAction(null, new { id = model.Id }, model);
        }
    }

}
