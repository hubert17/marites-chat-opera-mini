﻿// <auto-generated />
using System;
using ChatOperaMini.Models;
using EntityFrameworkCore.Jet.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ChatOperaMini.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240811115717_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Jet:ValueGenerationStrategy", JetValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ChatOperaMini.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Jet:ValueGenerationStrategy", JetValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ChannelCode")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("MessageText")
                        .HasColumnType("longchar");

                    b.Property<DateTime>("SendDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Sender")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Messages");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ChannelCode = "public",
                            MessageText = "Hi Zoey. I'll see you later.",
                            SendDate = new DateTime(2024, 8, 11, 19, 53, 16, 875, DateTimeKind.Local).AddTicks(6266),
                            Sender = "Mama"
                        },
                        new
                        {
                            Id = 2,
                            ChannelCode = "public",
                            MessageText = "Hi mama, our class is about to finish.",
                            SendDate = new DateTime(2024, 8, 11, 19, 54, 16, 875, DateTimeKind.Local).AddTicks(6280),
                            Sender = "Zoey"
                        },
                        new
                        {
                            Id = 3,
                            ChannelCode = "public",
                            MessageText = "I am driving home.",
                            SendDate = new DateTime(2024, 8, 11, 19, 55, 16, 875, DateTimeKind.Local).AddTicks(6281),
                            Sender = "Papa"
                        },
                        new
                        {
                            Id = 4,
                            ChannelCode = "public",
                            MessageText = "Zoey, are you there?",
                            SendDate = new DateTime(2024, 8, 11, 19, 57, 16, 875, DateTimeKind.Local).AddTicks(6282),
                            Sender = "Mama"
                        });
                });

            modelBuilder.Entity("ChatOperaMini.Models.MessageRead", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Jet:ValueGenerationStrategy", JetValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("MessageId")
                        .HasColumnType("integer");

                    b.Property<string>("Readby")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("MessageId");

                    b.ToTable("MessageReads");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            MessageId = 1,
                            Readby = "Zoey"
                        },
                        new
                        {
                            Id = 2,
                            MessageId = 2,
                            Readby = "Zoey"
                        },
                        new
                        {
                            Id = 3,
                            MessageId = 3,
                            Readby = "Zoey"
                        });
                });

            modelBuilder.Entity("ChatOperaMini.Models.MessageRead", b =>
                {
                    b.HasOne("ChatOperaMini.Models.Message", "Message")
                        .WithMany("MessageReads")
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Message");
                });

            modelBuilder.Entity("ChatOperaMini.Models.Message", b =>
                {
                    b.Navigation("MessageReads");
                });
#pragma warning restore 612, 618
        }
    }
}
