using Microsoft.EntityFrameworkCore.Migrations;

namespace ITP1.Migrations
{
    public partial class DropIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NekretninaImgs_NekretninaId",
                table: "NekretninaImgs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NekretninaImgs_NekretninaId",
                table: "NekretninaImgs");
        }
    }
}
