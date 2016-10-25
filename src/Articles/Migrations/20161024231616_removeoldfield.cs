using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Articles.Migrations
{
    public partial class removeoldfield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_BlogUser_BlogUserId1",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_BlogUserId1",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "BlogUserId1",
                table: "Posts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BlogUserId1",
                table: "Posts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_BlogUserId1",
                table: "Posts",
                column: "BlogUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_BlogUser_BlogUserId1",
                table: "Posts",
                column: "BlogUserId1",
                principalTable: "BlogUser",
                principalColumn: "BlogUserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
