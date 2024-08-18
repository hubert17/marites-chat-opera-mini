﻿// <auto-generated />
using System;
using ChatOperaMini.Models;
using EntityFrameworkCore.Jet.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ChatOperaMini.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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
                            SendDate = new DateTime(2024, 8, 18, 21, 59, 32, 961, DateTimeKind.Local).AddTicks(8283),
                            Sender = "Mama"
                        },
                        new
                        {
                            Id = 2,
                            ChannelCode = "public",
                            MessageText = "Hi mama, our class is about to finish.",
                            SendDate = new DateTime(2024, 8, 18, 22, 0, 32, 961, DateTimeKind.Local).AddTicks(8296),
                            Sender = "Zoey"
                        },
                        new
                        {
                            Id = 3,
                            ChannelCode = "public",
                            MessageText = "I am driving home.",
                            SendDate = new DateTime(2024, 8, 18, 22, 1, 32, 961, DateTimeKind.Local).AddTicks(8297),
                            Sender = "Papa"
                        },
                        new
                        {
                            Id = 4,
                            ChannelCode = "public",
                            MessageText = "Zoey, are you there?",
                            SendDate = new DateTime(2024, 8, 18, 22, 3, 32, 961, DateTimeKind.Local).AddTicks(8298),
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

            modelBuilder.Entity("ChatOperaMini.PushSubscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Jet:ValueGenerationStrategy", JetValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Auth")
                        .HasColumnType("longchar");

                    b.Property<string>("ChannelCode")
                        .HasColumnType("longchar");

                    b.Property<string>("Endpoint")
                        .HasColumnType("longchar");

                    b.Property<string>("P256DH")
                        .HasColumnType("longchar");

                    b.Property<string>("Sender")
                        .HasColumnType("longchar");

                    b.HasKey("Id");

                    b.ToTable("PushSubscriptions");
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
