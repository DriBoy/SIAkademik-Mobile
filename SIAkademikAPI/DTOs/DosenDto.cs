using System.ComponentModel.DataAnnotations;

namespace SIAkademikAPI.DTOs
{
    public class CreateDosenDto
    {
        [Required(ErrorMessage = "NIDN wajib diisi")]
        [StringLength(20, ErrorMessage = "NIDN maksimal 20 karakter")]
        public string Nidn { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nama Dosen wajib diisi")]
        [StringLength(100, ErrorMessage = "Nama Dosen maksimal 100 karakter")]
        public string NamaDosen { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email wajib diisi")]
        [EmailAddress(ErrorMessage = "Format email tidak valid")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Prodi wajib diisi")]
        [StringLength(50, ErrorMessage = "Prodi maksimal 50 karakter")]
        public string Prodi { get; set; } = string.Empty;
    }

    public class UpdateDosenDto : CreateDosenDto { }

}
