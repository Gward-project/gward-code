using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gwards.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRewardUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NftRewards_MinterAddress",
                table: "NftRewards");

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 18, 7, 44, 45, 197, DateTimeKind.Utc).AddTicks(160));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 18, 7, 44, 45, 197, DateTimeKind.Utc).AddTicks(220));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 18, 7, 44, 45, 197, DateTimeKind.Utc).AddTicks(240));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 18, 7, 44, 45, 197, DateTimeKind.Utc).AddTicks(250));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 18, 7, 44, 45, 197, DateTimeKind.Utc).AddTicks(250));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                column: "CreatedAt",
                value: new DateTime(2024, 9, 18, 6, 57, 11, 219, DateTimeKind.Utc).AddTicks(7300));

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

            migrationBuilder.CreateIndex(
                name: "IX_NftRewards_MinterAddress",
                table: "NftRewards",
                column: "MinterAddress",
                unique: true);
        }
    }
}
