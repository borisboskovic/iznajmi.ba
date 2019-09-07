using Microsoft.EntityFrameworkCore.Migrations;

namespace ITP1.Data.Migrations
{
    public partial class ColumnTypeChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "NekretninaImgs",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "PublicId",
                table: "NekretninaImgs",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Url",
                table: "NekretninaImgs",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PublicId",
                table: "NekretninaImgs",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
