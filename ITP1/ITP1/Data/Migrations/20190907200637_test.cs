using Microsoft.EntityFrameworkCore.Migrations;

namespace ITP1.Data.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NekretninaImgs_NekretninaId",
                table: "NekretninaImgs");

            migrationBuilder.CreateIndex(
                name: "IX_NekretninaImgs_NekretninaId",
                table: "NekretninaImgs",
                column: "NekretninaId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NekretninaImgs_NekretninaId",
                table: "NekretninaImgs");

            migrationBuilder.CreateIndex(
                name: "IX_NekretninaImgs_NekretninaId",
                table: "NekretninaImgs",
                column: "NekretninaId");
        }
    }
}
