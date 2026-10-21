
using System.ComponentModel.DataAnnotations;

namespace SIAkademikAPI.Models
{
    public class Mahasiswa
    {
        public int MahasiswaId { get; set; }

        public string Nim { get; set; } = string.Empty;

        public string NamaMahasiswa { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        [Range(2020, 2026)]
        public int Angkatan { get; set; }

        // Foreign Key ke Dosen
        public int DosenId { get; set; }

        public Dosen? Dosen { get; set; }
    }
}
