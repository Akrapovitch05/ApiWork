using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apiwork.Data;
using apiwork.Models;

namespace apiwork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly WorkConnectContext _context;

        public AdminController(WorkConnectContext context)
        {
            _context = context;
        }

        // GET: api/Admin
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Admin>>> GetAdmin()
        {
            return await _context.Admin.ToListAsync();  
        }

        // GET: api/Admin/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Admin>> GetAdmin(int id)
        {
            var admin = await _context.Admin.FindAsync(id); 

            if (admin == null)
            {
                return NotFound();
            }

            return admin;
        }

        // POST: api/Admin
        [HttpPost]
        public async Task<ActionResult<Admin>> PostAdmin(Admin admin)
        {
            _context.Admin.Add(admin);  // Corrected to plural
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdmin", new { id = admin.AdminID }, admin);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(Admin admin)
        {
            if (string.IsNullOrEmpty(admin.NomAdmin) || string.IsNullOrEmpty(admin.MotDePasseAdmin))
            {
                return BadRequest(new { message = "Nom d'utilisateur et mot de passe sont requis." });
            }

            var existingAdmin = await _context.Admin
                .FirstOrDefaultAsync(a => a.NomAdmin == admin.NomAdmin && a.MotDePasseAdmin == admin.MotDePasseAdmin);

            if (existingAdmin == null)
            {
                return Unauthorized(new { message = "Identifiants incorrects" });
            }

            // Renvoyer une réponse avec les informations de l'administrateur (sans le mot de passe)
            return Ok(new
            {
                adminID = existingAdmin.AdminID,
                nomAdmin = existingAdmin.NomAdmin,
                role = existingAdmin.Role
            });
        }



        // PUT: api/Admin/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdmin(int id, Admin admin)
        {
            if (id != admin.AdminID)
            {
                return BadRequest();
            }

            _context.Entry(admin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdminExists(id))
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

        // DELETE: api/Admin/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            var admin = await _context.Admin.FindAsync(id);  // Corrected to plural
            if (admin == null)
            {
                return NotFound();
            }

            _context.Admin.Remove(admin);  // Corrected to plural
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdminExists(int id)
        {
            return _context.Admin.Any(e => e.AdminID == id);  // Corrected to plural
        }
    }
}
