using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Articles.Migrations
{
    public partial class viewcount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAuthorSubscribes_BlogUser_authorId",
                table: "UserAuthorSubscribes");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAuthorSubscribes_BlogUser_userId",
                table: "UserAuthorSubscribes");

            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                table: "Posts",
                nullable: false,
                defaultValue: 0);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAuthorSubscribes_BlogUser_authorId",
                table: "UserAuthorSubscribes");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAuthorSubscribes_BlogUser_userId",
                table: "UserAuthorSubscribes");

            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "Posts");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAuthorSubscribes_BlogUser_authorId",
                table: "UserAuthorSubscribes",
                column: "authorId",
                principalTable: "BlogUser",
                principalColumn: "BlogUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAuthorSubscribes_BlogUser_userId",
                table: "UserAuthorSubscribes",
                column: "userId",
                principalTable: "BlogUser",
                principalColumn: "BlogUserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
