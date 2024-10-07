using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gwards.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMintQuest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NftQuestId",
                table: "Quests",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 18, 6, 57, 11, 219, DateTimeKind.Utc).AddTicks(7240));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 18, 6, 57, 11, 219, DateTimeKind.Utc).AddTicks(7290));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "NftQuestId" },
                values: new object[] { new DateTime(2024, 9, 18, 6, 57, 11, 219, DateTimeKind.Utc).AddTicks(7300), 2 });

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 18, 6, 57, 11, 219, DateTimeKind.Utc).AddTicks(7310));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 18, 6, 57, 11, 219, DateTimeKind.Utc).AddTicks(7320));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NftQuestId",
                table: "Quests");

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 17, 14, 6, 8, 44, DateTimeKind.Utc).AddTicks(7290));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 17, 14, 6, 8, 44, DateTimeKind.Utc).AddTicks(7350));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 17, 14, 6, 8, 44, DateTimeKind.Utc).AddTicks(7360));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 17, 14, 6, 8, 44, DateTimeKind.Utc).AddTicks(7380));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 17, 14, 6, 8, 44, DateTimeKind.Utc).AddTicks(7380));
        }
    }
}
