using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SlimeWeb.Core.Migrations.SqlServerMigrations
{
    public partial class addeddefaultvalueof10itemsperpageingenralsettinsmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ItemsPerPage",
                table: "Settings",
                type: "int",
                nullable: false,
                defaultValue: 10,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ItemsPerPage",
                table: "Settings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 10);
        }
    }
}
