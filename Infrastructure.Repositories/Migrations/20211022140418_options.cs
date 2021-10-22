using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Repositories.Migrations
{
    public partial class options : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "093a6402-3410-4bc5-a79a-2e3208b10929");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "dc2073b6-0c99-4811-9b25-5ba796d0f599");
        }
    }
}
