using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Articles.Migrations
{
    public partial class saves : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostUserSaves",
                columns: table => new
                {
                    PostId = table.Column<int>(nullable: false),
                    BlogUserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostUserSaves", x => new { x.PostId, x.BlogUserId });
                    table.ForeignKey(
                        name: "FK_PostUserSaves_BlogUser_BlogUserId",
                        column: x => x.BlogUserId,
                        principalTable: "BlogUser",
                        principalColumn: "BlogUserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostUserSaves_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddColumn<int>(
                name: "LikeCount",
                table: "Posts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PostUserSaves_BlogUserId",
                table: "PostUserSaves",
                column: "BlogUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PostUserSaves_PostId",
                table: "PostUserSaves",
                column: "PostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LikeCount",
                table: "Posts");

            migrationBuilder.DropTable(
                name: "PostUserSaves");
        }
    }
}
