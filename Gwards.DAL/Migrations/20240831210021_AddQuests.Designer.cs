﻿// <auto-generated />
using System;
using Gwards.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Gwards.DAL.Migrations
{
    [DbContext(typeof(GwardsContext))]
    [Migration("20240831210021_AddQuests")]
    partial class AddQuests
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Gwards.DAL.Entities.AccountLinkRequestEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Token")
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AccountLinkRequest");
                });

            modelBuilder.Entity("Gwards.DAL.Entities.ExternalAccountEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AccountId")
                        .HasColumnType("text");

                    b.Property<string>("AccountName")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Type")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ExternalAccounts");
                });

            modelBuilder.Entity("Gwards.DAL.Entities.Quests.QuestBaseEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("ImagePath")
                        .HasColumnType("text");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("boolean");

                    b.Property<int?>("Reward")
                        .HasColumnType("integer");

                    b.Property<string>("RewardNftImagePath")
                        .HasColumnType("text");

                    b.Property<int>("RewardType")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Quests");

                    b.HasDiscriminator<int>("Type");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Gwards.DAL.Entities.UserEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("GwardPointBalance")
                        .HasColumnType("integer");

                    b.Property<string>("Nickname")
                        .HasColumnType("text");

                    b.Property<int>("Score")
                        .HasColumnType("integer");

                    b.Property<long>("TelegramId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Gwards.DAL.Entities.UserQuestEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("ClaimedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("QuestId")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("QuestId", "UserId");

                    b.ToTable("UserQuests");
                });

            modelBuilder.Entity("Gwards.DAL.Entities.Quests.GameQuestEntity", b =>
                {
                    b.HasBaseType("Gwards.DAL.Entities.Quests.QuestBaseEntity");

                    b.HasDiscriminator().HasValue(2);

                    b.HasData(
                        new
                        {
                            Id = 4,
                            CreatedAt = new DateTime(2024, 8, 31, 21, 0, 21, 578, DateTimeKind.Utc).AddTicks(2520),
                            Description = "",
                            ImagePath = "/static/images/banana.png",
                            IsDefault = true,
                            RewardNftImagePath = "/static/images/nft.png",
                            RewardType = 1,
                            Title = "Play the banana game",
                            Type = 0
                        });
                });

            modelBuilder.Entity("Gwards.DAL.Entities.Quests.MintQuestEntity", b =>
                {
                    b.HasBaseType("Gwards.DAL.Entities.Quests.QuestBaseEntity");

                    b.HasDiscriminator().HasValue(3);

                    b.HasData(
                        new
                        {
                            Id = 3,
                            CreatedAt = new DateTime(2024, 8, 31, 21, 0, 21, 578, DateTimeKind.Utc).AddTicks(2500),
                            Description = "Go to our discord server and text the code word \"Let's Play\" to the bot to get the reward!",
                            ImagePath = "/static/images/banana.png",
                            IsDefault = true,
                            Reward = 50,
                            RewardType = 0,
                            Title = "Mint the Banana Achievement",
                            Type = 0
                        });
                });

            modelBuilder.Entity("Gwards.DAL.Entities.Quests.WalletQuestEntity", b =>
                {
                    b.HasBaseType("Gwards.DAL.Entities.Quests.QuestBaseEntity");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("Gwards.DAL.Entities.SubscribeQuestEntity", b =>
                {
                    b.HasBaseType("Gwards.DAL.Entities.Quests.QuestBaseEntity");

                    b.Property<int>("AccountType")
                        .HasColumnType("integer");

                    b.Property<string>("TargetAccount")
                        .HasColumnType("text");

                    b.HasDiscriminator().HasValue(0);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAt = new DateTime(2024, 8, 31, 21, 0, 21, 578, DateTimeKind.Utc).AddTicks(2429),
                            Description = "Go to our discord server and text the code word \"Let's Play\" to the bot to get the reward!",
                            ImagePath = "/static/images/discord.png",
                            IsDefault = true,
                            RewardNftImagePath = "/static/images/nft.png",
                            RewardType = 1,
                            Title = "Join our Discord server",
                            Type = 0,
                            AccountType = 0
                        },
                        new
                        {
                            Id = 2,
                            CreatedAt = new DateTime(2024, 8, 31, 21, 0, 21, 578, DateTimeKind.Utc).AddTicks(2432),
                            Description = "",
                            ImagePath = "/static/images/twitter.png",
                            IsDefault = true,
                            Reward = 50,
                            RewardType = 0,
                            Title = "Subscribe on Twitter",
                            Type = 0,
                            AccountType = 0
                        });
                });

            modelBuilder.Entity("Gwards.DAL.Entities.AccountLinkRequestEntity", b =>
                {
                    b.HasOne("Gwards.DAL.Entities.UserEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Gwards.DAL.Entities.ExternalAccountEntity", b =>
                {
                    b.HasOne("Gwards.DAL.Entities.UserEntity", "User")
                        .WithMany("ExternalAccounts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Gwards.DAL.Entities.UserQuestEntity", b =>
                {
                    b.HasOne("Gwards.DAL.Entities.Quests.QuestBaseEntity", "Quest")
                        .WithMany("UserQuests")
                        .HasForeignKey("QuestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Gwards.DAL.Entities.UserEntity", "User")
                        .WithMany("UserQuests")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Quest");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Gwards.DAL.Entities.Quests.QuestBaseEntity", b =>
                {
                    b.Navigation("UserQuests");
                });

            modelBuilder.Entity("Gwards.DAL.Entities.UserEntity", b =>
                {
                    b.Navigation("ExternalAccounts");

                    b.Navigation("UserQuests");
                });
#pragma warning restore 612, 618
        }
    }
}
