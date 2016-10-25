using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Articles.Migrations
{
    public partial class subscriptionjunction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAuthorSubscribes",
                columns: table => new
                {
                    authorId = table.Column<int>(nullable: false),
                    userId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAuthorSubscribes", x => new { x.authorId, x.userId });
                    table.ForeignKey(
                        name: "FK_UserAuthorSubscribes_BlogUser_authorId",
                        column: x => x.authorId,
                        principalTable: "BlogUser",
                        principalColumn: "BlogUserId",
                        onDelete: ReferentialAction.NoAction,
                         onUpdate: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_UserAuthorSubscribes_BlogUser_userId",
                        column: x => x.userId,
                        principalTable: "BlogUser",
                        principalColumn: "BlogUserId",
                        onDelete: ReferentialAction.NoAction,
                        onUpdate:  ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAuthorSubscribes_authorId",
                table: "UserAuthorSubscribes",
                column: "authorId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAuthorSubscribes_userId",
                table: "UserAuthorSubscribes",
                column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAuthorSubscribes");
        }
    }
}
