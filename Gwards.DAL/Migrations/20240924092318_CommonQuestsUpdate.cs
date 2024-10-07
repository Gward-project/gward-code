using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gwards.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CommonQuestsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 24, 9, 23, 18, 597, DateTimeKind.Utc).AddTicks(5370));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 24, 9, 23, 18, 597, DateTimeKind.Utc).AddTicks(5420));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 24, 9, 23, 18, 597, DateTimeKind.Utc).AddTicks(5440));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Description" },
                values: new object[] { new DateTime(2024, 9, 24, 9, 23, 18, 597, DateTimeKind.Utc).AddTicks(5450), "Join our Gward telegram channel at https://t.me/gwardxyz" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 20, 14, 48, 31, 219, DateTimeKind.Utc).AddTicks(9630));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 20, 14, 48, 31, 219, DateTimeKind.Utc).AddTicks(9680));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 20, 14, 48, 31, 219, DateTimeKind.Utc).AddTicks(9700));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Description" },
                values: new object[] { new DateTime(2024, 9, 20, 14, 48, 31, 219, DateTimeKind.Utc).AddTicks(9710), "Go to our telegram channel and text the code word \"Let's Play\" to the bot to get the reward!" });

            migrationBuilder.InsertData(
                table: "Quests",
                columns: new[] { "Id", "AccountType", "CreatedAt", "Description", "ImagePath", "IsDefault", "NftCollectionAddress", "NftImagePath", "NftMetadataBasePath", "Reward", "RewardType", "TargetAccount", "Title", "Type" },
                values: new object[] { 5, 0, new DateTime(2024, 9, 20, 14, 48, 31, 219, DateTimeKind.Utc).AddTicks(9710), "Subscribe to us on Twitter platform.", "/static/Images/twitter.png", true, null, null, null, 50, 0, null, "Subscribe on Twitter", 0 });
        }
    }
}
