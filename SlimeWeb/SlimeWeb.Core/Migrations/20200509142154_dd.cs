using Microsoft.EntityFrameworkCore.Migrations;

namespace SlimeWeb.Core.Migrations
{
    public partial class dd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RelativePath",
                table: "Files",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Administrator",
                table: "Blogs",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BlogMods",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlogId = table.Column<int>(nullable: false),
                    Moderator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogMods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogMods_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Post_BlogId",
                table: "Post",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_BlogId",
                table: "Files",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_Catgories_BlogId",
                table: "Catgories",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogMods_BlogId",
                table: "BlogMods",
                column: "BlogId");

            migrationBuilder.AddForeignKey(
                name: "FK_Catgories_Blogs_BlogId",
                table: "Catgories",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Blogs_BlogId",
                table: "Files",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Blogs_BlogId",
                table: "Post",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Catgories_Blogs_BlogId",
                table: "Catgories");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_Blogs_BlogId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_Blogs_BlogId",
                table: "Post");

            migrationBuilder.DropTable(
                name: "BlogMods");

            migrationBuilder.DropIndex(
                name: "IX_Post_BlogId",
                table: "Post");

            migrationBuilder.DropIndex(
                name: "IX_Files_BlogId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Catgories_BlogId",
                table: "Catgories");

            migrationBuilder.DropColumn(
                name: "RelativePath",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Administrator",
                table: "Blogs");
        }
    }
}
