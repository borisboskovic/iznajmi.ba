using Microsoft.EntityFrameworkCore.Migrations;

namespace ITP1.Data.Migrations
{
    public partial class removeCOl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NekretninaImgs_Nekretnine_NekretninaId1",
                table: "NekretninaImgs");

            migrationBuilder.DropIndex(
                name: "IX_NekretninaImgs_NekretninaId1",
                table: "NekretninaImgs");

            migrationBuilder.DropColumn(
                name: "NekretninaImgId",
                table: "Nekretnine");

            migrationBuilder.DropColumn(
                name: "NekretninaId1",
                table: "NekretninaImgs");


            migrationBuilder.AddForeignKey(
                name: "FK_NekretninaImgs_Nekretnine_NekretninaId",
                table: "NekretninaImgs",
                column: "NekretninaId",
                principalTable: "Nekretnine",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NekretninaImgs_Nekretnine_NekretninaId",
                table: "NekretninaImgs");

            migrationBuilder.DropIndex(
                name: "IX_NekretninaImgs_NekretninaId",
                table: "NekretninaImgs");

            migrationBuilder.AddColumn<int>(
                name: "NekretninaImgId",
                table: "Nekretnine",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NekretninaId1",
                table: "NekretninaImgs",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_NekretninaImgs_Nekretnine_NekretninaId1",
                table: "NekretninaImgs",
                column: "NekretninaId1",
                principalTable: "Nekretnine",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
