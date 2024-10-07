using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Gwards.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTonAddressesHandling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NftPassports_Users_UserId",
                table: "NftPassports");

            migrationBuilder.DropIndex(
                name: "IX_NftPassports_UserId",
                table: "NftPassports");

            migrationBuilder.DropColumn(
                name: "MintedNftAddress",
                table: "UserQuests");

            migrationBuilder.DropColumn(
                name: "MintedNftIndex",
                table: "UserQuests");

            migrationBuilder.DropColumn(
                name: "MinterAddress",
                table: "UserQuests");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "NftPassports");

            migrationBuilder.CreateTable(
                name: "NftRewards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Index = table.Column<long>(type: "bigint", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: true),
                    MinterAddress = table.Column<string>(type: "text", nullable: true),
                    QuestId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NftRewards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NftRewards_Quests_QuestId",
                        column: x => x.QuestId,
                        principalTable: "Quests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_NftPassports_MinterAddress",
                table: "NftPassports",
                column: "MinterAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NftRewards_MinterAddress",
                table: "NftRewards",
                column: "MinterAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NftRewards_QuestId",
                table: "NftRewards",
                column: "QuestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NftRewards");

            migrationBuilder.DropIndex(
                name: "IX_NftPassports_MinterAddress",
                table: "NftPassports");

            migrationBuilder.AddColumn<string>(
                name: "MintedNftAddress",
                table: "UserQuests",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MintedNftIndex",
                table: "UserQuests",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "MinterAddress",
                table: "UserQuests",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "NftPassports",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 17, 7, 34, 49, 717, DateTimeKind.Utc).AddTicks(4260));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 17, 7, 34, 49, 717, DateTimeKind.Utc).AddTicks(4310));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 17, 7, 34, 49, 717, DateTimeKind.Utc).AddTicks(4330));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 17, 7, 34, 49, 717, DateTimeKind.Utc).AddTicks(4350));

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 17, 7, 34, 49, 717, DateTimeKind.Utc).AddTicks(4350));

            migrationBuilder.CreateIndex(
                name: "IX_NftPassports_UserId",
                table: "NftPassports",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_NftPassports_Users_UserId",
                table: "NftPassports",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
