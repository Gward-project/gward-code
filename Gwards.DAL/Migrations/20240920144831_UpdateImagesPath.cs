using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gwards.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateImagesPath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ImagePath" },
                values: new object[] { new DateTime(2024, 9, 20, 14, 48, 31, 219, DateTimeKind.Utc).AddTicks(9630), "/static/Images/toncoin.png" });

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "ImagePath" },
                values: new object[] { new DateTime(2024, 9, 20, 14, 48, 31, 219, DateTimeKind.Utc).AddTicks(9680), "/static/Images/banana.png" });

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "ImagePath" },
                values: new object[] { new DateTime(2024, 9, 20, 14, 48, 31, 219, DateTimeKind.Utc).AddTicks(9700), "/static/Images/banana.png" });

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "ImagePath" },
                values: new object[] { new DateTime(2024, 9, 20, 14, 48, 31, 219, DateTimeKind.Utc).AddTicks(9710), "/static/Images/discord.png" });

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "ImagePath" },
                values: new object[] { new DateTime(2024, 9, 20, 14, 48, 31, 219, DateTimeKind.Utc).AddTicks(9710), "/static/Images/twitter.png" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ImagePath" },
                values: new object[] { new DateTime(2024, 9, 18, 7, 44, 45, 197, DateTimeKind.Utc).AddTicks(160), "/static/images/toncoin.png" });

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "ImagePath" },
                values: new object[] { new DateTime(2024, 9, 18, 7, 44, 45, 197, DateTimeKind.Utc).AddTicks(220), "/static/images/banana.png" });

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "ImagePath" },
                values: new object[] { new DateTime(2024, 9, 18, 7, 44, 45, 197, DateTimeKind.Utc).AddTicks(240), "/static/images/banana.png" });

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "ImagePath" },
                values: new object[] { new DateTime(2024, 9, 18, 7, 44, 45, 197, DateTimeKind.Utc).AddTicks(250), "/static/images/discord.png" });

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "ImagePath" },
                values: new object[] { new DateTime(2024, 9, 18, 7, 44, 45, 197, DateTimeKind.Utc).AddTicks(250), "/static/images/twitter.png" });
        }
    }
}
