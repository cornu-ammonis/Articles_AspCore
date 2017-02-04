using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Articles.Migrations
{
    public partial class fixsub : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAuthorSubscribes_BlogUser_authorId",
                table: "UserAuthorSubscribes");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAuthorSubscribes_BlogUser_userId",
                table: "UserAuthorSubscribes");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "UserAuthorSubscribes",
                newName: "userSubscribedId");

            migrationBuilder.RenameColumn(
                name: "authorId",
                table: "UserAuthorSubscribes",
                newName: "subscribingUserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAuthorSubscribes_userId",
                table: "UserAuthorSubscribes",
                newName: "IX_UserAuthorSubscribes_userSubscribedId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAuthorSubscribes_BlogUser_subscribingUserId",
                table: "UserAuthorSubscribes",
                column: "subscribingUserId",
                principalTable: "BlogUser",
                principalColumn: "BlogUserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAuthorSubscribes_BlogUser_userSubscribedId",
                table: "UserAuthorSubscribes",
                column: "userSubscribedId",
                principalTable: "BlogUser",
                principalColumn: "BlogUserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAuthorSubscribes_BlogUser_subscribingUserId",
                table: "UserAuthorSubscribes");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAuthorSubscribes_BlogUser_userSubscribedId",
                table: "UserAuthorSubscribes");

            migrationBuilder.RenameColumn(
                name: "userSubscribedId",
                table: "UserAuthorSubscribes",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "subscribingUserId",
                table: "UserAuthorSubscribes",
                newName: "authorId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAuthorSubscribes_userSubscribedId",
                table: "UserAuthorSubscribes",
                newName: "IX_UserAuthorSubscribes_userId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAuthorSubscribes_BlogUser_authorId",
                table: "UserAuthorSubscribes",
                column: "authorId",
                principalTable: "BlogUser",
                principalColumn: "BlogUserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAuthorSubscribes_BlogUser_userId",
                table: "UserAuthorSubscribes",
                column: "userId",
                principalTable: "BlogUser",
                principalColumn: "BlogUserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
