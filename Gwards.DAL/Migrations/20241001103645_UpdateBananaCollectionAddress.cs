using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gwards.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBananaCollectionAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 10, 1, 10, 36, 45, 726, DateTimeKind.Utc).AddTicks(2550));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "NftCollectionAddress" },
                values: new object[] { new DateTime(2024, 10, 1, 10, 36, 45, 726, DateTimeKind.Utc).AddTicks(2600), "EQDdFMFso8gYIaDxWvslY1pXnIoWeGIzLl8_hkWRuPk-xviT" });

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 10, 1, 10, 36, 45, 726, DateTimeKind.Utc).AddTicks(2620));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 10, 1, 10, 36, 45, 726, DateTimeKind.Utc).AddTicks(2630));
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
                columns: new[] { "CreatedAt", "NftCollectionAddress" },
                values: new object[] { new DateTime(2024, 9, 26, 20, 55, 1, 823, DateTimeKind.Utc).AddTicks(3220), "EQDnyVxmCoczrnS62y6mldV9IIWyNSFltqBCJ8HpuikudCfP" });

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
                column: "CreatedAt",
                value: new DateTime(2024, 9, 26, 20, 55, 1, 823, DateTimeKind.Utc).AddTicks(3250));
        }
    }
}
