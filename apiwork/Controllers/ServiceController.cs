using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apiwork.Data;
using apiwork.Models;

namespace apiwork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly WorkConnectContext _context;

        public ServiceController(WorkConnectContext context)
        {
            _context = context;
        }

        // GET: api/Service
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Service>>> GetServices()
        {
            return await _context.Service.ToListAsync();
        }

        // GET: api/Service?name=NomDuService
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Service>>> GetServicesByName(string name = null)
        {
            var query = _context.Service.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                // Filtre par nom de service
                query = query.Where(s => s.NomService.Contains(name)); // Utilisez Contains pour un filtrage partiel
            }

            var result = await query.ToListAsync();

            if (!result.Any())
            {
                return NotFound("Aucun service trouvé avec ce nom.");
            }

            return result;
        }


        // GET: api/Service/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Service>> GetService(int id)
        {
            var service = await _context.Service.FindAsync(id);

            if (service == null)
            {
                return NotFound();
            }

            return service;
        }

        // POST: api/Service
        [HttpPost]
        public async Task<ActionResult<Service>> PostService(Service service)
        {
            _context.Service.Add(service);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetService", new { id = service.ServiceID }, service);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutService(int id, [FromBody] Service service)
        {
            if (id != service.ServiceID)
            {
                return BadRequest("L'ID fourni dans l'URL ne correspond pas à l'ID de l'objet.");
            }

            // Vérification si le service existe avant modification
            var existingService = await _context.Service.FindAsync(id);
            if (existingService == null)
            {
                return NotFound("Le service demandé n'existe pas.");
            }

            // Mise à jour manuelle des propriétés si nécessaire
            existingService.NomService = service.NomService;
            existingService.Description = service.Description;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Erreur lors de la mise à jour du service.");
            }

            return Ok(existingService); // Retourne l'objet mis à jour
        }


        // DELETE: api/Service/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _context.Service.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            _context.Service.Remove(service);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServiceExists(int id)
        {
            return _context.Service.Any(e => e.ServiceID == id);
        }
    }
}
