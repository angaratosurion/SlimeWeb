using Microsoft.EntityFrameworkCore.Migrations;

namespace SlimeWeb.Core.Migrations
{
    public partial class addedconenttyeprpryonfilesmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Files",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Files");
        }
    }
}
