using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SlimeWeb.Core.Migrations.SQLServerMigrations.Migrations
{
    /// <inheritdoc />
    public partial class addedpost_namefiledonpost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Post",
                table: "Post");

            migrationBuilder.AddColumn<string>(
                name: "Post_Name",
                table: "Post",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Post",
                table: "Post",
                column: "Post_Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Post",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "Post_Name",
                table: "Post");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Post",
                table: "Post",
                column: "Id");
        }
    }
}
