using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIAkademikAPI.Data;
using SIAkademikAPI.DTOs;
using SIAkademikAPI.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SIAkademikAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DosenController : ControllerBase
    {
        private readonly AcademicDbContext _context;

        public DosenController(AcademicDbContext context)
        {
            _context = context;
        }

        // 1. GET: /api/dosen (Daftar Dosen)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dosen>>> GetDosens()
        {
            return await _context.Dosens.ToListAsync();
        }

        // 2. GET: /api/dosen/{id} (Detail Dosen)
        [HttpGet("{id}")]
        public async Task<ActionResult<Dosen>> GetDosen(int id)
        {
            var dosen = await _context.Dosens.FindAsync(id);

            if (dosen == null)
            {
                return NotFound(new { message = $"Dosen dengan ID {id} tidak ditemukan." });
            }

            return dosen;
        }

        // 3. POST: /api/dosen (Tambah Dosen)
        [HttpPost]
        public async Task<ActionResult<Dosen>> PostDosen(CreateDosenDto data)
        {
            // Validasi NIDN Unik
            if (await _context.Dosens.AnyAsync(d => d.Nidn == data.Nidn))
            {
                return BadRequest(new { message = "NIDN sudah terdaftar dalam sistem." });
            }

            var dosen = new Dosen
            {
                Nidn = data.Nidn,
                NamaDosen = data.NamaDosen,
                Email = data.Email,
                Prodi = data.Prodi
            };

            _context.Dosens.Add(dosen);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDosen), new { id = dosen.DosenId }, dosen);
        }

        // 4. PUT: /api/dosen/{id} (Ubah Dosen)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDosen(int id, UpdateDosenDto data)
        {
            var dosen = await _context.Dosens.FindAsync(id);
            if (dosen == null)
            {
                return NotFound(new { message = $"Dosen dengan ID {id} tidak ditemukan." });
            }

            // Validasi NIDN Unik jika NIDN diubah
            if (dosen.Nidn != data.Nidn && await _context.Dosens.AnyAsync(d => d.Nidn == data.Nidn))
            {
                return BadRequest(new { message = "NIDN sudah digunakan oleh dosen lain." });
            }

            dosen.Nidn = data.Nidn;
            dosen.NamaDosen = data.NamaDosen;
            dosen.Email = data.Email;
            dosen.Prodi = data.Prodi;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 5. DELETE: /api/dosen/{id} (Hapus Dosen)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDosen(int id)
        {
            var dosen = await _context.Dosens.FindAsync(id);

            if (dosen == null)
            {
                return NotFound(new
                {
                    message = $"Dosen dengan ID {id} tidak ditemukan."
                });
            }

            // Periksa apakah dosen masih menjadi wali mahasiswa
            var masihDigunakan = await _context.Mahasiswas
                .AnyAsync(m => m.DosenId == id);

            if (masihDigunakan)
            {
                return Conflict(new
                {
                    message = "Dosen tidak dapat dihapus karena masih menjadi dosen wali mahasiswa."
                });
            }

            _context.Dosens.Remove(dosen);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Data dosen berhasil dihapus."
            });
        }
    }
}
