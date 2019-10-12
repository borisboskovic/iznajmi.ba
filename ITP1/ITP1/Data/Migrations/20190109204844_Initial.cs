using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ITP1.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Komentari",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Tekst = table.Column<string>(nullable: true),
                    dateTime = table.Column<DateTime>(nullable: false),
                    NekretninaId = table.Column<string>(nullable: true),
                    KorisnikId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Komentari", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Korisnici",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Ime = table.Column<string>(nullable: true),
                    Tel = table.Column<string>(nullable: true),
                    MailKontakt = table.Column<string>(nullable: true),
                    WebKontaktUrl = table.Column<string>(nullable: true),
                    EMailFromAuthentication = table.Column<string>(nullable: true),
                    AvatarImgUrl = table.Column<string>(nullable: true),
                    NumberOfRatings5 = table.Column<int>(nullable: false),
                    NumberOfRatings4 = table.Column<int>(nullable: false),
                    NumberOfRatings3 = table.Column<int>(nullable: false),
                    NumberOfRatings2 = table.Column<int>(nullable: false),
                    NumberOfRatings1 = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnici", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Markeri",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Lat = table.Column<double>(nullable: false),
                    Lng = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Markeri", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tipovi",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ImeTipa = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipovi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Nekretnine",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naslov = table.Column<string>(nullable: true),
                    Lokacija = table.Column<string>(nullable: true),
                    Cijena = table.Column<int>(nullable: false),
                    Povrsina = table.Column<int>(nullable: false),
                    DostupnoOd = table.Column<DateTime>(nullable: false),
                    DostupnoDo = table.Column<DateTime>(nullable: false),
                    Opis = table.Column<string>(nullable: true),
                    TipId = table.Column<int>(nullable: false),
                    KorisnikId = table.Column<int>(nullable: false),
                    MarkerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nekretnine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nekretnine_Korisnici_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Nekretnine_Markeri_MarkerId",
                        column: x => x.MarkerId,
                        principalTable: "Markeri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Nekretnine_Tipovi_TipId",
                        column: x => x.TipId,
                        principalTable: "Tipovi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NekretninaImgs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Url = table.Column<int>(nullable: false),
                    PublicId = table.Column<int>(nullable: false),
                    NekretninaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NekretninaImgs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NekretninaImgs_Nekretnine_NekretninaId",
                        column: x => x.NekretninaId,
                        principalTable: "Nekretnine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            
            migrationBuilder.CreateIndex(
                name: "IX_Nekretnine_KorisnikId",
                table: "Nekretnine",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Nekretnine_MarkerId",
                table: "Nekretnine",
                column: "MarkerId");

            migrationBuilder.CreateIndex(
                name: "IX_Nekretnine_TipId",
                table: "Nekretnine",
                column: "TipId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Komentari");

            migrationBuilder.DropTable(
                name: "NekretninaImgs");

            migrationBuilder.DropTable(
                name: "Nekretnine");

            migrationBuilder.DropTable(
                name: "Korisnici");

            migrationBuilder.DropTable(
                name: "Markeri");

            migrationBuilder.DropTable(
                name: "Tipovi");
        }
    }
}
