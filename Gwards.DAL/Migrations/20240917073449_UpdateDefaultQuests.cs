using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Gwards.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDefaultQuests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Description", "ImagePath", "NftCollectionAddress", "NftImagePath", "NftMetadataBasePath", "Reward", "RewardType", "Title", "Type" },
                values: new object[] { new DateTime(2024, 9, 17, 7, 34, 49, 717, DateTimeKind.Utc).AddTicks(4260), "Connect you're ton wallet to Gward's telegram mini app", "/static/images/toncoin.png", null, null, null, 50, 0, "Connect ton wallet", 1 });

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ApplicationId", "CreatedAt", "Description", "ImagePath", "NftCollectionAddress", "NftImagePath", "NftMetadataBasePath", "Platform", "Reward", "RewardType", "Title", "Type" },
                values: new object[] { 2923300L, new DateTime(2024, 9, 17, 7, 34, 49, 717, DateTimeKind.Utc).AddTicks(4310), "Play the banana game from you're steam library", "/static/images/banana.png", "EQDnyVxmCoczrnS62y6mldV9IIWyNSFltqBCJ8HpuikudCfP", "/static/NftImages/BananaNFT.png", "/static/NftCollections/BananaCollection", "steam", null, 1, "Play the banana game", 2 });

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Description", "Reward" },
                values: new object[] { new DateTime(2024, 9, 17, 7, 34, 49, 717, DateTimeKind.Utc).AddTicks(4330), "Mint NFT achievement from 'Play the banana game' quest", 200 });

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "AccountType", "CreatedAt", "Description", "ImagePath", "NftCollectionAddress", "NftImagePath", "NftMetadataBasePath", "Reward", "RewardType", "TargetAccount", "Title", "Type" },
                values: new object[] { 0, new DateTime(2024, 9, 17, 7, 34, 49, 717, DateTimeKind.Utc).AddTicks(4350), "Go to our telegram channel and text the code word \"Let's Play\" to the bot to get the reward!", "/static/images/discord.png", null, null, null, 50, 0, null, "Join our Telegram channel", 0 });

            migrationBuilder.InsertData(
                table: "Quests",
                columns: new[] { "Id", "AccountType", "CreatedAt", "Description", "ImagePath", "IsDefault", "NftCollectionAddress", "NftImagePath", "NftMetadataBasePath", "Reward", "RewardType", "TargetAccount", "Title", "Type" },
                values: new object[] { 5, 0, new DateTime(2024, 9, 17, 7, 34, 49, 717, DateTimeKind.Utc).AddTicks(4350), "Subscribe to us on Twitter platform.", "/static/images/twitter.png", true, null, null, null, 50, 0, null, "Subscribe on Twitter", 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Description", "Reward" },
                values: new object[] { new DateTime(2024, 9, 17, 6, 51, 32, 974, DateTimeKind.Utc).AddTicks(4900), "Go to our discord server and text the code word \"Let's Play\" to the bot to get the reward!", 50 });

            migrationBuilder.InsertData(
                table: "Quests",
                columns: new[] { "Id", "AccountType", "CreatedAt", "Description", "ImagePath", "IsDefault", "NftCollectionAddress", "NftImagePath", "NftMetadataBasePath", "Reward", "RewardType", "TargetAccount", "Title", "Type" },
                values: new object[,]
                {
                    { 1, 0, new DateTime(2024, 9, 17, 6, 51, 32, 974, DateTimeKind.Utc).AddTicks(4840), "Go to our discord server and text the code word \"Let's Play\" to the bot to get the reward!", "/static/images/discord.png", true, "EQDnyVxmCoczrnS62y6mldV9IIWyNSFltqBCJ8HpuikudCfP", "/static/NftImages/BananaNFT.png", "/static/NftCollections/BananaCollection", null, 1, null, "Join our Discord server", 0 },
                    { 2, 0, new DateTime(2024, 9, 17, 6, 51, 32, 974, DateTimeKind.Utc).AddTicks(4850), "", "/static/images/twitter.png", true, null, null, null, 50, 0, null, "Subscribe on Twitter", 0 }
                });

            migrationBuilder.InsertData(
                table: "Quests",
                columns: new[] { "Id", "ApplicationId", "CreatedAt", "Description", "ImagePath", "IsDefault", "NftCollectionAddress", "NftImagePath", "NftMetadataBasePath", "Platform", "Reward", "RewardType", "Title", "Type" },
                values: new object[] { 4, 2923300L, new DateTime(2024, 9, 17, 6, 51, 32, 974, DateTimeKind.Utc).AddTicks(4910), "", "/static/images/banana.png", true, "EQDnyVxmCoczrnS62y6mldV9IIWyNSFltqBCJ8HpuikudCfP", "/static/NftImages/BananaNFT.png", "/static/NftCollections/BananaCollection", "steam", null, 1, "Play the banana game", 2 });
        }
    }
}
