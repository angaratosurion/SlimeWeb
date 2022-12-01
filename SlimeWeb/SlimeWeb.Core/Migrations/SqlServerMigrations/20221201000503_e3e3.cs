using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SlimeWeb.Core.Migrations.SqlServerMigrations
{
    public partial class e3e3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Catgories",
                table: "Catgories");

            migrationBuilder.AddColumn<string>(
                name: "BlogAndTag",
                table: "Tags",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BlogAndCategory",
                table: "Catgories",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                table: "Tags",
                column: "BlogAndTag");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Catgories",
                table: "Catgories",
                column: "BlogAndCategory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Catgories",
                table: "Catgories");

            migrationBuilder.DropColumn(
                name: "BlogAndTag",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "BlogAndCategory",
                table: "Catgories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                table: "Tags",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Catgories",
                table: "Catgories",
                column: "Id");
        }
    }
}
