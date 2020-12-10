using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SlimeWeb.Core.Migrations
{
    public partial class newsmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NewsId",
                table: "Tags",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NewsId",
                table: "Catgories",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Published = table.Column<DateTime>(nullable: false),
                    content = table.Column<string>(nullable: true),
                    Author = table.Column<string>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tags_NewsId",
                table: "Tags",
                column: "NewsId");

            migrationBuilder.CreateIndex(
                name: "IX_Catgories_NewsId",
                table: "Catgories",
                column: "NewsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Catgories_News_NewsId",
                table: "Catgories",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_News_NewsId",
                table: "Tags",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Catgories_News_NewsId",
                table: "Catgories");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_News_NewsId",
                table: "Tags");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropIndex(
                name: "IX_Tags_NewsId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Catgories_NewsId",
                table: "Catgories");

            migrationBuilder.DropColumn(
                name: "NewsId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "NewsId",
                table: "Catgories");
        }
    }
}
