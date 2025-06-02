using GameNetManagment.Core.Entities;
using GameNetManagment.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameNetManagment.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StationsController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Station>>> GetStations()
        {
            return await _context.Stations.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Station>> PostStation(Station station)
        {
            _context.Stations.Add(station);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetStation), new { id = station.Id }, station); // نیاز به متد GetStation(id) دارید
        }

        // یک متد GetStation(id) هم اضافه کنید
        [HttpGet("{id}")]
        public async Task<ActionResult<Station>> GetStation(int id)
        {
            var station = await _context.Stations.FindAsync(id);
            if (station == null)
            {
                return NotFound();
            }
            return station;
        }

    }
}
