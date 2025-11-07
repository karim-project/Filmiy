using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Filmiy.Migrations
{
    /// <inheritdoc />
    public partial class AddCinemaModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "cinema",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "cinema",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfHalls",
                table: "cinema",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "cinema");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "cinema");

            migrationBuilder.DropColumn(
                name: "NumberOfHalls",
                table: "cinema");
        }
    }
}
