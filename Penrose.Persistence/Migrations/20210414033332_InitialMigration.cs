using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Penrose.Persistence.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "User",
                table => new
                {
                    Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                    Version = table.Column<Guid>("uniqueidentifier", nullable: false),
                    Name = table.Column<string>("nvarchar(max)", nullable: false),
                    LastName = table.Column<string>("nvarchar(max)", nullable: false),
                    Email = table.Column<string>("nvarchar(max)", nullable: false),
                    Hash = table.Column<string>("nvarchar(max)", nullable: false),
                    Is2FaEnabled = table.Column<bool>("bit", nullable: false, defaultValue: false),
                    IsEmailVerified = table.Column<bool>("bit", nullable: false, defaultValue: false),
                    LastLogin = table.Column<DateTime>("datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>("datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>("datetime2", nullable: false),
                    IsActive = table.Column<bool>("bit", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_User", x => x.Id); });

            migrationBuilder.CreateTable(
                "Chat",
                table => new
                {
                    Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>("uniqueidentifier", nullable: true),
                    Version = table.Column<Guid>("uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>("datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>("datetime2", nullable: false),
                    IsActive = table.Column<bool>("bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chat", x => x.Id);
                    table.ForeignKey(
                        "FK_Chat_User_UserId",
                        x => x.UserId,
                        "User",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "ChatMessage",
                table => new
                {
                    Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                    ChatId = table.Column<Guid>("uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>("uniqueidentifier", nullable: false),
                    Content = table.Column<string>("nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>("nvarchar(max)", nullable: false),
                    IsSeen = table.Column<bool>("bit", nullable: false, defaultValue: false),
                    IsDeleted = table.Column<bool>("bit", nullable: false, defaultValue: false),
                    DateSeen = table.Column<DateTime>("datetime2", nullable: true),
                    DateDeleted = table.Column<DateTime>("datetime2", nullable: true),
                    Version = table.Column<Guid>("uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>("datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>("datetime2", nullable: false),
                    IsActive = table.Column<bool>("bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessage", x => x.Id);
                    table.ForeignKey(
                        "FK_ChatMessage_Chat_ChatId",
                        x => x.ChatId,
                        "Chat",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_ChatMessage_User_UserId",
                        x => x.UserId,
                        "User",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "ChatParticipants",
                table => new
                {
                    Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>("uniqueidentifier", nullable: false),
                    ChatId = table.Column<Guid>("uniqueidentifier", nullable: false),
                    Version = table.Column<Guid>("uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>("datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>("datetime2", nullable: false),
                    IsActive = table.Column<bool>("bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatParticipants", x => x.Id);
                    table.ForeignKey(
                        "FK_ChatParticipants_Chat_ChatId",
                        x => x.ChatId,
                        "Chat",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_ChatParticipants_User_UserId",
                        x => x.UserId,
                        "User",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_Chat_UserId",
                "Chat",
                "UserId");

            migrationBuilder.CreateIndex(
                "IX_ChatMessage_ChatId",
                "ChatMessage",
                "ChatId");

            migrationBuilder.CreateIndex(
                "IX_ChatMessage_UserId",
                "ChatMessage",
                "UserId");

            migrationBuilder.CreateIndex(
                "IX_ChatParticipants_ChatId",
                "ChatParticipants",
                "ChatId");

            migrationBuilder.CreateIndex(
                "IX_ChatParticipants_UserId",
                "ChatParticipants",
                "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "ChatMessage");

            migrationBuilder.DropTable(
                "ChatParticipants");

            migrationBuilder.DropTable(
                "Chat");

            migrationBuilder.DropTable(
                "User");
        }
    }
}