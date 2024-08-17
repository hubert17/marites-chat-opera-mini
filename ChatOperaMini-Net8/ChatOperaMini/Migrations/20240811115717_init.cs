using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ChatOperaMini.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Jet:Identity", "1, 1"),
                    ChannelCode = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    Sender = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    MessageText = table.Column<string>(type: "longchar", nullable: true),
                    SendDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MessageReads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Jet:Identity", "1, 1"),
                    MessageId = table.Column<int>(type: "integer", nullable: false),
                    Readby = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageReads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageReads_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Messages",
                columns: new[] { "Id", "ChannelCode", "MessageText", "SendDate", "Sender" },
                values: new object[,]
                {
                    { 1, "public", "Hi Zoey. I'll see you later.", new DateTime(2024, 8, 11, 19, 53, 16, 875, DateTimeKind.Local).AddTicks(6266), "Mama" },
                    { 2, "public", "Hi mama, our class is about to finish.", new DateTime(2024, 8, 11, 19, 54, 16, 875, DateTimeKind.Local).AddTicks(6280), "Zoey" },
                    { 3, "public", "I am driving home.", new DateTime(2024, 8, 11, 19, 55, 16, 875, DateTimeKind.Local).AddTicks(6281), "Papa" },
                    { 4, "public", "Zoey, are you there?", new DateTime(2024, 8, 11, 19, 57, 16, 875, DateTimeKind.Local).AddTicks(6282), "Mama" }
                });

            migrationBuilder.InsertData(
                table: "MessageReads",
                columns: new[] { "Id", "MessageId", "Readby" },
                values: new object[,]
                {
                    { 1, 1, "Zoey" },
                    { 2, 2, "Zoey" },
                    { 3, 3, "Zoey" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MessageReads_MessageId",
                table: "MessageReads",
                column: "MessageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageReads");

            migrationBuilder.DropTable(
                name: "Messages");
        }
    }
}
