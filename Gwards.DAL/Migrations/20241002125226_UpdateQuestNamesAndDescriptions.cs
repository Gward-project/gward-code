using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gwards.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateQuestNamesAndDescriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 10, 2, 12, 52, 26, 286, DateTimeKind.Utc).AddTicks(9020));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Description", "Title" },
                values: new object[] { new DateTime(2024, 10, 2, 12, 52, 26, 286, DateTimeKind.Utc).AddTicks(9080), "Play banana game from Steam", "Play banana game" });

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Description", "Title" },
                values: new object[] { new DateTime(2024, 10, 2, 12, 52, 26, 286, DateTimeKind.Utc).AddTicks(9100), "Mint NFT achievement from 'Play banana game' quest", "Mint banana achievement" });

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 10, 2, 12, 52, 26, 286, DateTimeKind.Utc).AddTicks(9110));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                columns: new[] { "CreatedAt", "Description", "Title" },
                values: new object[] { new DateTime(2024, 9, 26, 20, 55, 1, 823, DateTimeKind.Utc).AddTicks(3220), "Play the banana game from you're steam library", "Play the banana game" });

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Description", "Title" },
                values: new object[] { new DateTime(2024, 9, 26, 20, 55, 1, 823, DateTimeKind.Utc).AddTicks(3230), "Mint NFT achievement from 'Play the banana game' quest", "Mint the Banana Achievement" });

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 26, 20, 55, 1, 823, DateTimeKind.Utc).AddTicks(3250));
        }
    }
}
