using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SlimeWeb.Core.Migrations
{
    public partial class deletedfletypetabe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_FileTypes_FileTypeId",
                table: "Files");

            migrationBuilder.DropTable(
                name: "FileTypes");

            migrationBuilder.DropIndex(
                name: "IX_Files_FileTypeId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "FileTypeId",
                table: "Files");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FileTypeId",
                table: "Files",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FileTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Extention = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Files_FileTypeId",
                table: "Files",
                column: "FileTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_FileTypes_FileTypeId",
                table: "Files",
                column: "FileTypeId",
                principalTable: "FileTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
