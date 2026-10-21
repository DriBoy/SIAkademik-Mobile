using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIAkademikAPI.Data;
using SIAkademikAPI.DTOs;
using SIAkademikAPI.Models;

namespace SIAkademikAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MahasiswaController : ControllerBase
    {
        private readonly AcademicDbContext _context;

        public MahasiswaController(AcademicDbContext context)
        {
            _context = context;
        }

        // 1. GET: /api/mahasiswa (Daftar Mahasiswa + Data Dosen Wali)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetMahasiswas()
        {
            return await _context.Mahasiswas
                .Include(m => m.Dosen)
                .Select(m => new
                {
                    m.MahasiswaId,
                    m.Nim,
                    m.NamaMahasiswa,
                    m.Email,
                    m.Angkatan,
                    m.DosenId,
                    DosenWali = m.Dosen != null ? m.Dosen.NamaDosen : null
                })
                .ToListAsync();
        }

        // 2. GET: /api/mahasiswa/{id} (Detail Mahasiswa)
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetMahasiswa(int id)
        {
            var m = await _context.Mahasiswas
                .Include(m => m.Dosen)
                .FirstOrDefaultAsync(x => x.MahasiswaId == id);

            if (m == null)
            {
                return NotFound(new { message = $"Mahasiswa dengan ID {id} tidak ditemukan." });
            }

            return new
            {
                m.MahasiswaId,
                m.Nim,
                m.NamaMahasiswa,
                m.Email,
                m.Angkatan,
                m.DosenId,
                DosenWali = m.Dosen != null ? m.Dosen.NamaDosen : null
            };
        }

        // 3. POST: /api/mahasiswa (Tambah Mahasiswa)
        [HttpPost]
        public async Task<ActionResult<Mahasiswa>> PostMahasiswa(CreateMahasiswaDto data)
        {
            // Validasi NIM Unik
            if (await _context.Mahasiswas.AnyAsync(m => m.Nim == data.Nim))
            {
                return BadRequest(new { message = "NIM sudah terdaftar dalam sistem." });
            }

            // Validasi keberadaan DosenId
            var dosenExists = await _context.Dosens.AnyAsync(d => d.DosenId == data.DosenId);
            if (!dosenExists)
            {
                return BadRequest(new { message = $"Dosen dengan ID {data.DosenId} tidak ditemukan." });
            }

            var mhs = new Mahasiswa
            {
                Nim = data.Nim,
                NamaMahasiswa = data.NamaMahasiswa,
                Email = data.Email,
                Angkatan = data.Angkatan,
                DosenId = data.DosenId
            };

            _context.Mahasiswas.Add(mhs);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMahasiswa), new { id = mhs.MahasiswaId }, mhs);
        }

        // 4. PUT: /api/mahasiswa/{id} (Ubah Mahasiswa)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMahasiswa(int id, UpdateMahasiswaDto data)
        {
            var mhs = await _context.Mahasiswas.FindAsync(id);
            if (mhs == null)
            {
                return NotFound(new { message = $"Mahasiswa dengan ID {id} tidak ditemukan." });
            }

            // Validasi NIM Unik jika NIM diubah
            if (mhs.Nim != data.Nim && await _context.Mahasiswas.AnyAsync(m => m.Nim == data.Nim))
            {
                return BadRequest(new { message = "NIM sudah digunakan oleh mahasiswa lain." });
            }

            // Validasi keberadaan DosenId
            var dosenExists = await _context.Dosens.AnyAsync(d => d.DosenId == data.DosenId);
            if (!dosenExists)
            {
                return BadRequest(new { message = $"Dosen dengan ID {data.DosenId} tidak ditemukan." });
            }

            mhs.Nim = data.Nim;
            mhs.NamaMahasiswa = data.NamaMahasiswa;
            mhs.Email = data.Email;
            mhs.Angkatan = data.Angkatan;
            mhs.DosenId = data.DosenId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 5. DELETE: /api/mahasiswa/{id} (Hapus Mahasiswa)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMahasiswa(int id)
        {
            var mhs = await _context.Mahasiswas.FindAsync(id);
            if (mhs == null)
            {
                return NotFound(new { message = $"Mahasiswa dengan ID {id} tidak ditemukan." });
            }

            _context.Mahasiswas.Remove(mhs);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Data mahasiswa berhasil dihapus." });
        }
    }
}