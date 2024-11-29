using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apiwork.Data;
using apiwork.Models;

namespace apiwork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoriqueModificationController : ControllerBase
    {
        private readonly WorkConnectContext _context;

        public HistoriqueModificationController(WorkConnectContext context)
        {
            _context = context;
        }

        // GET: api/HistoriqueModification
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HistoriqueModification>>> GetHistoriqueModifications()
        {
            return await _context.HistoriqueModification.ToListAsync();
        }

        // GET: api/HistoriqueModification/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HistoriqueModification>> GetHistoriqueModification(int id)
        {
            var historiqueModification = await _context.HistoriqueModification.FindAsync(id);

            if (historiqueModification == null)
            {
                return NotFound();
            }

            return historiqueModification;
        }

        // POST: api/HistoriqueModification
        [HttpPost]
        public async Task<ActionResult<HistoriqueModification>> PostHistoriqueModification(HistoriqueModification historiqueModification)
        {
            _context.HistoriqueModification.Add(historiqueModification);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHistoriqueModification", new { id = historiqueModification.HistoriqueID }, historiqueModification);
        }

        // DELETE: api/HistoriqueModification/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHistoriqueModification(int id)
        {
            var historiqueModification = await _context.HistoriqueModification.FindAsync(id);
            if (historiqueModification == null)
            {
                return NotFound();
            }

            _context.HistoriqueModification.Remove(historiqueModification);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
