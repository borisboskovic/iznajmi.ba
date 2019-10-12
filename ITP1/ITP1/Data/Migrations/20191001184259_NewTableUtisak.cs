using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ITP1.Data.Migrations
{
    public partial class NewTableUtisak : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Utisci",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    KorisnikId = table.Column<int>(nullable: false),
                    OcjenjeniKorinsnikid = table.Column<int>(nullable: false),
                    Komentar = table.Column<string>(nullable: true),
                    Ocjena = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utisci", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Utisci_Korisnici_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Utisci_KorisnikId",
                table: "Utisci",
                column: "KorisnikId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Utisci");
        }
    }
}
