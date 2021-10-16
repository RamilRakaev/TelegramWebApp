using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Infrastructure.Repositories.Migrations
{
    public partial class Option : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TelegramUsers_TelegramOptions_TelegramOptionsId",
                table: "TelegramUsers");

            migrationBuilder.DropTable(
                name: "CalendarOptions");

            migrationBuilder.DropTable(
                name: "TelegramOptions");

            migrationBuilder.DropIndex(
                name: "IX_TelegramUsers_TelegramOptionsId",
                table: "TelegramUsers");

            migrationBuilder.DropColumn(
                name: "TelegramOptionsId",
                table: "TelegramUsers");

            migrationBuilder.CreateTable(
                name: "Options",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PropertyName = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Options", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "e58bcc70-166b-477d-863a-efd296bc50a7");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Options");

            migrationBuilder.AddColumn<int>(
                name: "TelegramOptionsId",
                table: "TelegramUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CalendarOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApiKey = table.Column<string>(type: "text", nullable: true),
                    CalandarId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TelegramOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Token = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelegramOptions", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "c5c1ab22-96a0-42a3-b7b9-86188f0946f7");

            migrationBuilder.CreateIndex(
                name: "IX_TelegramUsers_TelegramOptionsId",
                table: "TelegramUsers",
                column: "TelegramOptionsId");

            migrationBuilder.AddForeignKey(
                name: "FK_TelegramUsers_TelegramOptions_TelegramOptionsId",
                table: "TelegramUsers",
                column: "TelegramOptionsId",
                principalTable: "TelegramOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
