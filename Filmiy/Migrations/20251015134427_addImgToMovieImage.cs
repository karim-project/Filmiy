using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Filmiy.Migrations
{
    /// <inheritdoc />
    public partial class addImgToMovieImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Img",
                table: "movieImages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Img",
                table: "movieImages");
        }
    }
}
