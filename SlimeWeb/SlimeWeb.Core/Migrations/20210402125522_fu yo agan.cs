using Microsoft.EntityFrameworkCore.Migrations;

namespace SlimeWeb.Core.Migrations
{
    public partial class fuyoagan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Files");

            migrationBuilder.CreateTable(
                name: "FilesPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    FileId = table.Column<int>(type: "int", nullable: false),
                    BlogId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilesPosts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilesPosts");

            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
