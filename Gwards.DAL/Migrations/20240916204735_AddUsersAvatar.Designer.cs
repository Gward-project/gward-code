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
    [Migration("20240916204735_AddUsersAvatar")]
    partial class AddUsersAvatar
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

            modelBuilder.Entity("Gwards.DAL.Entities.DailyRewardEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Day")
                        .HasColumnType("integer");

                    b.Property<DateTime>("LastRewardClaimedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("DailyRewards");
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

            modelBuilder.Entity("Gwards.DAL.Entities.NftPassportEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ImagePath")
                        .HasColumnType("text");

                    b.Property<long>("Index")
                        .HasColumnType("bigint");

                    b.Property<string>("MetadataPath")
                        .HasColumnType("text");

                    b.Property<int>("PaymentId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Address")
                        .IsUnique();

                    b.HasIndex("PaymentId")
                        .IsUnique();

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("NftPassports");
                });

            modelBuilder.Entity("Gwards.DAL.Entities.PaymentEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("PostPaymentMethod")
                        .HasColumnType("integer");

                    b.Property<int?>("TransactionId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TransactionId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Payments");
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

                    b.Property<string>("NftCollectionAddress")
                        .HasColumnType("text");

                    b.Property<string>("NftImagePath")
                        .HasColumnType("text");

                    b.Property<string>("NftMetadataBasePath")
                        .HasColumnType("text");

                    b.Property<int?>("Reward")
                        .HasColumnType("integer");

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

            modelBuilder.Entity("Gwards.DAL.Entities.TransactionEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Destination")
                        .HasColumnType("text");

                    b.Property<string>("Hash")
                        .HasColumnType("text");

                    b.Property<string>("Source")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Hash")
                        .IsUnique();

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("Gwards.DAL.Entities.UserEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("AvatarFetchedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("AvatarPath")
                        .HasColumnType("text");

                    b.Property<long?>("ChatId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("GwardPointBalance")
                        .HasColumnType("integer");

                    b.Property<bool>("IsReferralCodeApplied")
                        .HasColumnType("boolean");

                    b.Property<string>("Nickname")
                        .HasColumnType("text");

                    b.Property<string>("ReferralCode")
                        .HasColumnType("text");

                    b.Property<int>("Score")
                        .HasColumnType("integer");

                    b.Property<long>("TelegramId")
                        .HasColumnType("bigint");

                    b.Property<string>("TonAddress")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("TonAddress")
                        .IsUnique();

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

                    b.Property<string>("MintedNftAddress")
                        .HasColumnType("text");

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

                    b.Property<long>("ApplicationId")
                        .HasColumnType("bigint");

                    b.Property<string>("Platform")
                        .HasColumnType("text");

                    b.HasDiscriminator().HasValue(2);

                    b.HasData(
                        new
                        {
                            Id = 4,
                            CreatedAt = new DateTime(2024, 9, 16, 20, 47, 35, 28, DateTimeKind.Utc).AddTicks(9680),
                            Description = "",
                            ImagePath = "/static/images/banana.png",
                            IsDefault = true,
                            NftCollectionAddress = "EQDnyVxmCoczrnS62y6mldV9IIWyNSFltqBCJ8HpuikudCfP",
                            NftImagePath = "/static/NftImages/BananaNFT.png",
                            NftMetadataBasePath = "/static/NftCollections/BananaCollection",
                            RewardType = 1,
                            Title = "Play the banana game",
                            Type = 0,
                            ApplicationId = 2923300L,
                            Platform = "steam"
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
                            CreatedAt = new DateTime(2024, 9, 16, 20, 47, 35, 28, DateTimeKind.Utc).AddTicks(9670),
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
                            CreatedAt = new DateTime(2024, 9, 16, 20, 47, 35, 28, DateTimeKind.Utc).AddTicks(9610),
                            Description = "Go to our discord server and text the code word \"Let's Play\" to the bot to get the reward!",
                            ImagePath = "/static/images/discord.png",
                            IsDefault = true,
                            NftCollectionAddress = "EQDnyVxmCoczrnS62y6mldV9IIWyNSFltqBCJ8HpuikudCfP",
                            NftImagePath = "/static/NftImages/BananaNFT.png",
                            NftMetadataBasePath = "/static/NftCollections/BananaCollection",
                            RewardType = 1,
                            Title = "Join our Discord server",
                            Type = 0,
                            AccountType = 0
                        },
                        new
                        {
                            Id = 2,
                            CreatedAt = new DateTime(2024, 9, 16, 20, 47, 35, 28, DateTimeKind.Utc).AddTicks(9610),
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

            modelBuilder.Entity("Gwards.DAL.Entities.DailyRewardEntity", b =>
                {
                    b.HasOne("Gwards.DAL.Entities.UserEntity", "User")
                        .WithOne("DailyReward")
                        .HasForeignKey("Gwards.DAL.Entities.DailyRewardEntity", "UserId")
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

            modelBuilder.Entity("Gwards.DAL.Entities.NftPassportEntity", b =>
                {
                    b.HasOne("Gwards.DAL.Entities.PaymentEntity", "Payment")
                        .WithOne()
                        .HasForeignKey("Gwards.DAL.Entities.NftPassportEntity", "PaymentId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Gwards.DAL.Entities.UserEntity", "User")
                        .WithOne("Passport")
                        .HasForeignKey("Gwards.DAL.Entities.NftPassportEntity", "UserId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("Payment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Gwards.DAL.Entities.PaymentEntity", b =>
                {
                    b.HasOne("Gwards.DAL.Entities.TransactionEntity", "Transaction")
                        .WithOne()
                        .HasForeignKey("Gwards.DAL.Entities.PaymentEntity", "TransactionId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("Gwards.DAL.Entities.UserEntity", "User")
                        .WithMany("Payments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Transaction");

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
                    b.Navigation("DailyReward");

                    b.Navigation("ExternalAccounts");

                    b.Navigation("Passport");

                    b.Navigation("Payments");

                    b.Navigation("UserQuests");
                });
#pragma warning restore 612, 618
        }
    }
}
