using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apiwork.Data;
using apiwork.Models;

namespace apiwork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteController : ControllerBase
    {
        private readonly WorkConnectContext _context;

        public SiteController(WorkConnectContext context)
        {
            _context = context;
        }

        // GET: api/Site
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Site>>> GetSites()
        {
            return await _context.Site.ToListAsync();
        }

        // GET: api/Site/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Site>> GetSite(int id)
        {
            var site = await _context.Site.FindAsync(id);

            if (site == null)
            {
                return NotFound();
            }

            return site;
        }

        // POST: api/Site
        [HttpPost]
        public async Task<ActionResult<Site>> PostSite(Site site)
        {
            _context.Site.Add(site);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSite", new { id = site.SiteID }, site);
        }

        // PUT: api/Site/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSite(int id, Site site)
        {
            if (id != site.SiteID)
            {
                return BadRequest();
            }

            _context.Entry(site).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SiteExists(id))
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

        // DELETE: api/Site/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSite(int id)
        {
            var site = await _context.Site.FindAsync(id);
            if (site == null)
            {
                return NotFound();
            }

            // Vérifier si le site a des dépendants
            var hasDependents = await HasDependentsOnSite(id);
            if (hasDependents)
            {
                return BadRequest("Impossible de supprimer ce site car il est encore utilisé par des enregistrements.");
            }

            _context.Site.Remove(site);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Méthode pour vérifier si un site a des dépendants
        private async Task<bool> HasDependentsOnSite(int siteId)
        {
            // Vérifier si des salariés sont liés à ce site
            var dependentsCount = await _context.Salarie.CountAsync(s => s.SiteID == siteId);
            return dependentsCount > 0;
        }

        private bool SiteExists(int id)
        {
            return _context.Site.Any(e => e.SiteID == id);
        }
    }
}
