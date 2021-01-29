using Microsoft.EntityFrameworkCore.Migrations;

namespace SlimeWeb.Core.Migrations
{
    public partial class ffefefefe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogMods_Blogs_BlogId",
                table: "BlogMods");

            migrationBuilder.DropForeignKey(
                name: "FK_Catgories_Blogs_BlogId",
                table: "Catgories");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_Blogs_BlogId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_Blogs_BlogId",
                table: "Post");

            migrationBuilder.DropIndex(
                name: "IX_Post_BlogId",
                table: "Post");

            migrationBuilder.DropIndex(
                name: "IX_Files_BlogId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Catgories_BlogId",
                table: "Catgories");

            migrationBuilder.AlterColumn<int>(
                name: "BlogId",
                table: "BlogMods",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogMods_Blogs_BlogId",
                table: "BlogMods",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogMods_Blogs_BlogId",
                table: "BlogMods");

            migrationBuilder.AlterColumn<int>(
                name: "BlogId",
                table: "BlogMods",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_BlogMods_Blogs_BlogId",
                table: "BlogMods",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
    }
}
