using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apiwork.Data;
using apiwork.Models;

namespace apiwork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalarieController : ControllerBase
    {
        private readonly WorkConnectContext _context;

        public SalarieController(WorkConnectContext context)
        {
            _context = context;
        }

        // GET: api/Salarie
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Salarie>>> GetSalaries(string name = null, int? siteId = null, int? serviceId = null)
        {
            var query = _context.Salarie
                                .Include(s => s.Service)
                                .Include(s => s.Site)
                                .AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(s => s.Nom.Contains(name));
            }

            if (siteId.HasValue)
            {
                query = query.Where(s => s.SiteID == siteId.Value);
            }

            if (serviceId.HasValue)
            {
                query = query.Where(s => s.ServiceID == serviceId.Value);
            }

            var salaries = await query.ToListAsync();
            return Ok(salaries);
        }

        // GET: api/Salaries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Salarie>> GetSalarie(int id)
        {
            // Inclure les entités liées (Service et Site)
            var salarie = await _context.Salarie
                                        .Include(s => s.Service)
                                        .Include(s => s.Site)
                                        .FirstOrDefaultAsync(s => s.SalarieID == id);

            if (salarie == null)
            {
                return NotFound();
            }

            return salarie;
        }

        // GET: api/Salaries/search?name=John
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Salarie>>> SearchSalarie(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return await _context.Salarie.ToListAsync();
            }

            var filteredSalaries = await _context.Salarie
                .Where(s => s.Nom.Contains(name))
                .ToListAsync();

            return filteredSalaries;
        }

        // POST: api/Salaries
        [HttpPost]
        public async Task<ActionResult<Salarie>> PostSalarie([FromBody] Salarie salarie)
        {
            if (salarie == null)
            {
                return BadRequest("Invalid data.");
            }

            // Récupérer les entités associées (Site et Service) en fonction de leurs IDs
            var site = await _context.Site.FindAsync(salarie.SiteID);
            var service = await _context.Service.FindAsync(salarie.ServiceID);

            if (site == null || service == null)
            {
                return BadRequest("Site ou Service non trouvé.");
            }

            // Associer les entités Site et Service au salarié
            salarie.Site = site;
            salarie.Service = service;

            // Ajouter le salarié à la base de données
            _context.Salarie.Add(salarie);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSalarie), new { id = salarie.SalarieID }, salarie);
        }

        // PUT: api/Salaries/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSalarie(int id, Salarie salarie)
        {
            if (id != salarie.SalarieID)
            {
                return BadRequest();
            }

            _context.Entry(salarie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalarieExists(id))
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

        // DELETE: api/Salaries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalarie(int id)
        {
            var salarie = await _context.Salarie.FindAsync(id);
            if (salarie == null)
            {
                return NotFound();
            }

            _context.Salarie.Remove(salarie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SalarieExists(int id)
        {
            return _context.Salarie.Any(e => e.SalarieID == id);
        }
    }
}
