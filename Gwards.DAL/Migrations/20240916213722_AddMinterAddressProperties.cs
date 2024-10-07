using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gwards.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddMinterAddressProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NftPassports_UserId",
                table: "NftPassports");

            migrationBuilder.AddColumn<string>(
                name: "MinterAddress",
                table: "UserQuests",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MinterAddress",
                table: "NftPassports",
                type: "text",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_NftPassports_UserId",
                table: "NftPassports",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NftPassports_UserId",
                table: "NftPassports");

            migrationBuilder.DropColumn(
                name: "MinterAddress",
                table: "UserQuests");

            migrationBuilder.DropColumn(
                name: "MinterAddress",
                table: "NftPassports");

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 16, 21, 17, 18, 747, DateTimeKind.Utc).AddTicks(5520));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 16, 21, 17, 18, 747, DateTimeKind.Utc).AddTicks(5530));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 16, 21, 17, 18, 747, DateTimeKind.Utc).AddTicks(5590));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 16, 21, 17, 18, 747, DateTimeKind.Utc).AddTicks(5610));

            migrationBuilder.CreateIndex(
                name: "IX_NftPassports_UserId",
                table: "NftPassports",
                column: "UserId",
                unique: true);
        }
    }
}
