using Microsoft.EntityFrameworkCore.Migrations;

namespace SlimeWeb.Core.Migrations.SqlServerMigrations
{
    public partial class addedactivepropertytotheblogmodsmobel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "BlogMods",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "BlogMods");
        }
    }
}
