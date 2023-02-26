using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SlimeWeb.Core.Migrations.SqlServerMigrations
{
    public partial class aaa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Direction",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderBy",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Direction",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "OrderBy",
                table: "Settings");
        }
    }
}
