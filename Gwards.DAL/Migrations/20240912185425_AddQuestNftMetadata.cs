using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gwards.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddQuestNftMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RewardNftImagePath",
                table: "Quests",
                newName: "NftMetadataBasePath");

            migrationBuilder.AddColumn<string>(
                name: "NftCollectionAddress",
                table: "Quests",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NftImagePath",
                table: "Quests",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "NftCollectionAddress", "NftImagePath", "NftMetadataBasePath" },
                values: new object[] { new DateTime(2024, 9, 12, 18, 54, 25, 782, DateTimeKind.Utc).AddTicks(1990), "EQDnyVxmCoczrnS62y6mldV9IIWyNSFltqBCJ8HpuikudCfP", "/static/NftImages/BananaNFT.png", "/static/NftCollections/BananaCollection" });

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "NftCollectionAddress", "NftImagePath" },
                values: new object[] { new DateTime(2024, 9, 12, 18, 54, 25, 782, DateTimeKind.Utc).AddTicks(1990), null, null });

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "NftCollectionAddress", "NftImagePath" },
                values: new object[] { new DateTime(2024, 9, 12, 18, 54, 25, 782, DateTimeKind.Utc).AddTicks(2040), null, null });

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "NftCollectionAddress", "NftImagePath", "NftMetadataBasePath" },
                values: new object[] { new DateTime(2024, 9, 12, 18, 54, 25, 782, DateTimeKind.Utc).AddTicks(2050), "EQDnyVxmCoczrnS62y6mldV9IIWyNSFltqBCJ8HpuikudCfP", "/static/NftImages/BananaNFT.png", "/static/NftCollections/BananaCollection" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NftCollectionAddress",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "NftImagePath",
                table: "Quests");

            migrationBuilder.RenameColumn(
                name: "NftMetadataBasePath",
                table: "Quests",
                newName: "RewardNftImagePath");

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "RewardNftImagePath" },
                values: new object[] { new DateTime(2024, 9, 8, 13, 11, 41, 117, DateTimeKind.Utc).AddTicks(3740), "/static/images/nft.png" });

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
                columns: new[] { "CreatedAt", "RewardNftImagePath" },
                values: new object[] { new DateTime(2024, 9, 8, 13, 11, 41, 117, DateTimeKind.Utc).AddTicks(3839), "/static/images/nft.png" });
        }
    }
}
