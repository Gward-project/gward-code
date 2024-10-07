using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gwards.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddMintedNftIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "MintedNftIndex",
                table: "UserQuests",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 16, 22, 7, 37, 688, DateTimeKind.Utc).AddTicks(7160));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 16, 22, 7, 37, 688, DateTimeKind.Utc).AddTicks(7170));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 16, 22, 7, 37, 688, DateTimeKind.Utc).AddTicks(7240));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 16, 22, 7, 37, 688, DateTimeKind.Utc).AddTicks(7250));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MintedNftIndex",
                table: "UserQuests");

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 16, 21, 37, 22, 690, DateTimeKind.Utc).AddTicks(5820));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 16, 21, 37, 22, 690, DateTimeKind.Utc).AddTicks(5820));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 16, 21, 37, 22, 690, DateTimeKind.Utc).AddTicks(5880));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 16, 21, 37, 22, 690, DateTimeKind.Utc).AddTicks(5890));
        }
    }
}
