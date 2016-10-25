using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Articles.Migrations
{
    public partial class removeoldfield2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_BlogUser_BlogUserId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_BlogUserId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "BlogUserId",
                table: "Posts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BlogUserId",
                table: "Posts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_BlogUserId",
                table: "Posts",
                column: "BlogUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_BlogUser_BlogUserId",
                table: "Posts",
                column: "BlogUserId",
                principalTable: "BlogUser",
                principalColumn: "BlogUserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
