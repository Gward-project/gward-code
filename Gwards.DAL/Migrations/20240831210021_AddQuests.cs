using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Gwards.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddQuests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Nickname",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "GwardPointBalance",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "ExternalAccounts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "AccountName",
                table: "ExternalAccounts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "AccountId",
                table: "ExternalAccounts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "AccountLinkRequest",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "AccountLinkRequest",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "Quests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false),
                    RewardType = table.Column<int>(type: "integer", nullable: false),
                    Reward = table.Column<int>(type: "integer", nullable: true),
                    RewardNftImagePath = table.Column<string>(type: "text", nullable: true),
                    TargetAccount = table.Column<string>(type: "text", nullable: true),
                    AccountType = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserQuests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    QuestId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ClaimedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserQuests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserQuests_Quests_QuestId",
                        column: x => x.QuestId,
                        principalTable: "Quests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserQuests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Quests",
                columns: new[] { "Id", "AccountType", "CreatedAt", "Description", "ImagePath", "IsDefault", "Reward", "RewardNftImagePath", "RewardType", "TargetAccount", "Title", "Type" },
                values: new object[,]
                {
                    { 1, 0, new DateTime(2024, 8, 31, 21, 0, 21, 578, DateTimeKind.Utc).AddTicks(2429), "Go to our discord server and text the code word \"Let's Play\" to the bot to get the reward!", "/static/images/discord.png", true, null, "/static/images/nft.png", 1, null, "Join our Discord server", 0 },
                    { 2, 0, new DateTime(2024, 8, 31, 21, 0, 21, 578, DateTimeKind.Utc).AddTicks(2432), "", "/static/images/twitter.png", true, 50, null, 0, null, "Subscribe on Twitter", 0 }
                });

            migrationBuilder.InsertData(
                table: "Quests",
                columns: new[] { "Id", "CreatedAt", "Description", "ImagePath", "IsDefault", "Reward", "RewardNftImagePath", "RewardType", "Title", "Type" },
                values: new object[,]
                {
                    { 3, new DateTime(2024, 8, 31, 21, 0, 21, 578, DateTimeKind.Utc).AddTicks(2500), "Go to our discord server and text the code word \"Let's Play\" to the bot to get the reward!", "/static/images/banana.png", true, 50, null, 0, "Mint the Banana Achievement", 3 },
                    { 4, new DateTime(2024, 8, 31, 21, 0, 21, 578, DateTimeKind.Utc).AddTicks(2520), "", "/static/images/banana.png", true, null, "/static/images/nft.png", 1, "Play the banana game", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountLinkRequest_UserId",
                table: "AccountLinkRequest",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuests_QuestId_UserId",
                table: "UserQuests",
                columns: new[] { "QuestId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserQuests_UserId",
                table: "UserQuests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountLinkRequest_Users_UserId",
                table: "AccountLinkRequest",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountLinkRequest_Users_UserId",
                table: "AccountLinkRequest");

            migrationBuilder.DropTable(
                name: "UserQuests");

            migrationBuilder.DropTable(
                name: "Quests");

            migrationBuilder.DropIndex(
                name: "IX_AccountLinkRequest_UserId",
                table: "AccountLinkRequest");

            migrationBuilder.DropColumn(
                name: "GwardPointBalance",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Nickname",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "ExternalAccounts",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountName",
                table: "ExternalAccounts",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountId",
                table: "ExternalAccounts",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "AccountLinkRequest",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "AccountLinkRequest",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
