using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gwards.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTelegramQuestImagePath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 26, 20, 55, 1, 823, DateTimeKind.Utc).AddTicks(3140));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 26, 20, 55, 1, 823, DateTimeKind.Utc).AddTicks(3220));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 26, 20, 55, 1, 823, DateTimeKind.Utc).AddTicks(3230));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "ImagePath" },
                values: new object[] { new DateTime(2024, 9, 26, 20, 55, 1, 823, DateTimeKind.Utc).AddTicks(3250), "/static/Images/telegram.png" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 24, 9, 32, 22, 42, DateTimeKind.Utc).AddTicks(200));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 24, 9, 32, 22, 42, DateTimeKind.Utc).AddTicks(250));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 24, 9, 32, 22, 42, DateTimeKind.Utc).AddTicks(270));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "ImagePath" },
                values: new object[] { new DateTime(2024, 9, 24, 9, 32, 22, 42, DateTimeKind.Utc).AddTicks(280), "/static/Images/discord.png" });
        }
    }
}
