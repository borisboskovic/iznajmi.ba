using Microsoft.EntityFrameworkCore.Migrations;

namespace ITP1.Data.Migrations
{
    public partial class ChangeOfColumnType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfRatings1",
                table: "Korisnici");

            migrationBuilder.DropColumn(
                name: "NumberOfRatings2",
                table: "Korisnici");

            migrationBuilder.DropColumn(
                name: "NumberOfRatings3",
                table: "Korisnici");

            migrationBuilder.DropColumn(
                name: "NumberOfRatings4",
                table: "Korisnici");

            migrationBuilder.DropColumn(
                name: "NumberOfRatings5",
                table: "Korisnici");

            migrationBuilder.AlterColumn<int>(
                name: "NekretninaId",
                table: "Komentari",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "KorisnikId",
                table: "Komentari",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfRatings1",
                table: "Korisnici",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfRatings2",
                table: "Korisnici",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfRatings3",
                table: "Korisnici",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfRatings4",
                table: "Korisnici",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfRatings5",
                table: "Korisnici",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "NekretninaId",
                table: "Komentari",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "KorisnikId",
                table: "Komentari",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
