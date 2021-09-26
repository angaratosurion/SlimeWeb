using Microsoft.EntityFrameworkCore.Migrations;

namespace SlimeWeb.Core.Migrations.SqlServerMigrations
{
    public partial class achangedblogmods : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogMods_Blogs_BlogId",
                table: "BlogMods");

            migrationBuilder.DropIndex(
                name: "IX_BlogMods_BlogId",
                table: "BlogMods");

            migrationBuilder.RenameColumn(
                name: "Moderator",
                table: "BlogMods",
                newName: "ModeratorId");

            migrationBuilder.AlterColumn<int>(
                name: "BlogId",
                table: "BlogMods",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModeratorId",
                table: "BlogMods",
                newName: "Moderator");

            migrationBuilder.AlterColumn<int>(
                name: "BlogId",
                table: "BlogMods",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_BlogMods_BlogId",
                table: "BlogMods",
                column: "BlogId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogMods_Blogs_BlogId",
                table: "BlogMods",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
