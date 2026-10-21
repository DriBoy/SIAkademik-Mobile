namespace SIAkademikAPI.Models
{
    public class Dosen
    {
        public int DosenId { get; set; }
        public string Nidn { get; set; } = string.Empty;
        public string NamaDosen { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Prodi { get; set; } = string.Empty;

        // Relasi: Satu Dosen bisa membimbing banyak Mahasiswa (One-to-Many)
        public ICollection<Mahasiswa>? Mahasiswas { get; set; }
    }
}
