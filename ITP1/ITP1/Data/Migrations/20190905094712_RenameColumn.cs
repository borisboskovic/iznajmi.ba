using Microsoft.EntityFrameworkCore.Migrations;

namespace ITP1.Data.Migrations
{
    public partial class RenameColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nekretnine_NacinIznajmljivnja_NacinIznajmljivanjaId1",
                table: "Nekretnine");

            migrationBuilder.DropIndex(
                name: "IX_Nekretnine_NacinIznajmljivanjaId1",
                table: "Nekretnine");

            migrationBuilder.DropColumn(
                name: "NacinIznajmljivanjaId1",
                table: "Nekretnine");

            migrationBuilder.AlterColumn<int>(
                name: "NacinIznajmljivanjaId",
                table: "Nekretnine",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nekretnine_NacinIznajmljivanjaId",
                table: "Nekretnine",
                column: "NacinIznajmljivanjaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Nekretnine_NacinIznajmljivnja_NacinIznajmljivanjaId",
                table: "Nekretnine",
                column: "NacinIznajmljivanjaId",
                principalTable: "NacinIznajmljivnja",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nekretnine_NacinIznajmljivnja_NacinIznajmljivanjaId",
                table: "Nekretnine");

            migrationBuilder.DropIndex(
                name: "IX_Nekretnine_NacinIznajmljivanjaId",
                table: "Nekretnine");

            migrationBuilder.AlterColumn<string>(
                name: "NacinIznajmljivanjaId",
                table: "Nekretnine",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "NacinIznajmljivanjaId1",
                table: "Nekretnine",
                nullable: true);

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
    }
}
