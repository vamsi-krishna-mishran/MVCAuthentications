using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCApplicationAlongWithWebAPI.Migrations
{
    public partial class addedImagefield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Img",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Img",
                table: "Books");
        }
    }
}
