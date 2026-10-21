using Microsoft.EntityFrameworkCore;
using SIAkademikAPI.Models;

namespace SIAkademikAPI.Data
{
    public class AcademicDbContext : DbContext
    {
        public AcademicDbContext(DbContextOptions<AcademicDbContext> options) : base(options) { }

        // 1. Mendaftarkan DbSet
        public DbSet<Dosen> Dosens => Set<Dosen>();
        public DbSet<Mahasiswa> Mahasiswas => Set<Mahasiswa>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 2. Mengatur Primary Key
            modelBuilder.Entity<Dosen>().HasKey(d => d.DosenId);
            modelBuilder.Entity<Mahasiswa>().HasKey(m => m.MahasiswaId);

            // 3. Konfigurasi Unique Constraint untuk NIDN dan NIM
            modelBuilder.Entity<Dosen>()
                .HasIndex(d => d.Nidn)
                .IsUnique();

            modelBuilder.Entity<Mahasiswa>()
                .HasIndex(m => m.Nim)
                .IsUnique();

            // 2. Mengatur relasi One-to-Many (1 Dosen -> Banyak Mahasiswa)
            modelBuilder.Entity<Mahasiswa>()
                .HasOne(m => m.Dosen)
                .WithMany(d => d.Mahasiswas)
                .HasForeignKey(m => m.DosenId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
