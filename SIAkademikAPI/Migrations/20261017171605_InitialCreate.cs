using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIAkademikAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dosens",
                columns: table => new
                {
                    DosenId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nidn = table.Column<string>(type: "TEXT", nullable: false),
                    NamaDosen = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Prodi = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dosens", x => x.DosenId);
                });

            migrationBuilder.CreateTable(
                name: "Mahasiswas",
                columns: table => new
                {
                    MahasiswaId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nim = table.Column<string>(type: "TEXT", nullable: false),
                    NamaMahasiswa = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Angkatan = table.Column<int>(type: "INTEGER", nullable: false),
                    DosenId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mahasiswas", x => x.MahasiswaId);
                    table.ForeignKey(
                        name: "FK_Mahasiswas_Dosens_DosenId",
                        column: x => x.DosenId,
                        principalTable: "Dosens",
                        principalColumn: "DosenId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dosens_Nidn",
                table: "Dosens",
                column: "Nidn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mahasiswas_DosenId",
                table: "Mahasiswas",
                column: "DosenId");

            migrationBuilder.CreateIndex(
                name: "IX_Mahasiswas_Nim",
                table: "Mahasiswas",
                column: "Nim",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mahasiswas");

            migrationBuilder.DropTable(
                name: "Dosens");
        }
    }
}
