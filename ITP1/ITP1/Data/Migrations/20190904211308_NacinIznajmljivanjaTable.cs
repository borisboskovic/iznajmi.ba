using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ITP1.Data.Migrations
{
    public partial class NacinIznajmljivanjaTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NacinIznajmljivanjaId",
                table: "Nekretnine",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NacinIznajmljivanjaId1",
                table: "Nekretnine",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "NacinIznajmljivnja",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NacinIznajmljivnja", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Nekretnine_NacinIznajmljivanjaId1",
                table: "Nekretnine",
                column: "NacinIznajmljivanjaId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Nekretnine_NacinIznajmljivnja_NacinIznajmljivanjaId1",
                table: "Nekretnine",
                column: "NacinIznajmljivanjaId1",
                principalTable: "NacinIznajmljivnja",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nekretnine_NacinIznajmljivnja_NacinIznajmljivanjaId1",
                table: "Nekretnine");

            migrationBuilder.DropTable(
                name: "NacinIznajmljivnja");

            migrationBuilder.DropIndex(
                name: "IX_Nekretnine_NacinIznajmljivanjaId1",
                table: "Nekretnine");

            migrationBuilder.DropColumn(
                name: "NacinIznajmljivanjaId",
                table: "Nekretnine");

            migrationBuilder.DropColumn(
                name: "NacinIznajmljivanjaId1",
                table: "Nekretnine");
        }
    }
}
