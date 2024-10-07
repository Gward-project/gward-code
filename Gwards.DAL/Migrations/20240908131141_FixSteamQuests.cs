using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gwards.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixSteamQuests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ApplicationId",
                table: "Quests",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Platform",
                table: "Quests",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 8, 13, 11, 41, 117, DateTimeKind.Utc).AddTicks(3740));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 8, 13, 11, 41, 117, DateTimeKind.Utc).AddTicks(3743));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 8, 13, 11, 41, 117, DateTimeKind.Utc).AddTicks(3825));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ApplicationId", "CreatedAt", "Platform" },
                values: new object[] { 2923300L, new DateTime(2024, 9, 8, 13, 11, 41, 117, DateTimeKind.Utc).AddTicks(3839), "steam" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "Platform",
                table: "Quests");

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 8, 31, 21, 0, 21, 578, DateTimeKind.Utc).AddTicks(2429));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 8, 31, 21, 0, 21, 578, DateTimeKind.Utc).AddTicks(2432));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 8, 31, 21, 0, 21, 578, DateTimeKind.Utc).AddTicks(2500));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 8, 31, 21, 0, 21, 578, DateTimeKind.Utc).AddTicks(2520));
        }
    }
}
