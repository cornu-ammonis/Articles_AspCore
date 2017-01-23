using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Articles.Migrations
{
    public partial class blockingauthorizing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserAuthorSubscribes_authorId",
                table: "UserAuthorSubscribes");

            migrationBuilder.DropIndex(
                name: "IX_PostUserSaves_PostId",
                table: "PostUserSaves");

            migrationBuilder.DropIndex(
                name: "IX_PostUserLikes_PostId",
                table: "PostUserLikes");

            migrationBuilder.DropIndex(
                name: "IX_PostTag_PostId",
                table: "PostTag");

            migrationBuilder.DropIndex(
                name: "IX_CategoryBlogUser_CategoryId",
                table: "CategoryBlogUser");

            migrationBuilder.CreateTable(
                name: "UserAuthorizesUsers",
                columns: table => new
                {
                    authorizingUserId = table.Column<int>(nullable: false),
                    userAuthorizedId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAuthorizesUsers", x => new { x.authorizingUserId, x.userAuthorizedId });
                    table.ForeignKey(
                        name: "FK_UserAuthorizesUsers_BlogUser_authorizingUserId",
                        column: x => x.authorizingUserId,
                        principalTable: "BlogUser",
                        principalColumn: "BlogUserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserAuthorizesUsers_BlogUser_userAuthorizedId",
                        column: x => x.userAuthorizedId,
                        principalTable: "BlogUser",
                        principalColumn: "BlogUserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserBlocksUsers",
                columns: table => new
                {
                    blockingUserId = table.Column<int>(nullable: false),
                    userBlockedId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBlocksUsers", x => new { x.blockingUserId, x.userBlockedId });
                    table.ForeignKey(
                        name: "FK_UserBlocksUsers_BlogUser_blockingUserId",
                        column: x => x.blockingUserId,
                        principalTable: "BlogUser",
                        principalColumn: "BlogUserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserBlocksUsers_BlogUser_userBlockedId",
                        column: x => x.userBlockedId,
                        principalTable: "BlogUser",
                        principalColumn: "BlogUserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAuthorizesUsers_userAuthorizedId",
                table: "UserAuthorizesUsers",
                column: "userAuthorizedId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBlocksUsers_userBlockedId",
                table: "UserBlocksUsers",
                column: "userBlockedId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAuthorizesUsers");

            migrationBuilder.DropTable(
                name: "UserBlocksUsers");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName");

            migrationBuilder.CreateIndex(
                name: "IX_UserAuthorSubscribes_authorId",
                table: "UserAuthorSubscribes",
                column: "authorId");

            migrationBuilder.CreateIndex(
                name: "IX_PostUserSaves_PostId",
                table: "PostUserSaves",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostUserLikes_PostId",
                table: "PostUserLikes",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostTag_PostId",
                table: "PostTag",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryBlogUser_CategoryId",
                table: "CategoryBlogUser",
                column: "CategoryId");
        }
    }
}
