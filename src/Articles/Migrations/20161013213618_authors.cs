using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Articles.Migrations
{
    public partial class authors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthorBlogUserId",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BlogUserId1",
                table: "BlogUser",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_AuthorBlogUserId",
                table: "Posts",
                column: "AuthorBlogUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogUser_BlogUserId1",
                table: "BlogUser",
                column: "BlogUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogUser_BlogUser_BlogUserId1",
                table: "BlogUser",
                column: "BlogUserId1",
                principalTable: "BlogUser",
                principalColumn: "BlogUserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_BlogUser_AuthorBlogUserId",
                table: "Posts",
                column: "AuthorBlogUserId",
                principalTable: "BlogUser",
                principalColumn: "BlogUserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogUser_BlogUser_BlogUserId1",
                table: "BlogUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_BlogUser_AuthorBlogUserId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_AuthorBlogUserId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_BlogUser_BlogUserId1",
                table: "BlogUser");

            migrationBuilder.DropColumn(
                name: "AuthorBlogUserId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "BlogUserId1",
                table: "BlogUser");
        }
    }
}
