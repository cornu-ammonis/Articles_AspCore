using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Articles.Migrations
{
    public partial class sad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogUser_BlogUser_BlogUserId",
                table: "BlogUser");

            migrationBuilder.DropIndex(
                name: "IX_BlogUser_BlogUserId",
                table: "BlogUser");

            migrationBuilder.AddColumn<int>(
                name: "BlogUserId1",
                table: "BlogUser",
                nullable: true);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogUser_BlogUser_BlogUserId1",
                table: "BlogUser");

            migrationBuilder.DropIndex(
                name: "IX_BlogUser_BlogUserId1",
                table: "BlogUser");

            migrationBuilder.DropColumn(
                name: "BlogUserId1",
                table: "BlogUser");

            migrationBuilder.CreateIndex(
                name: "IX_BlogUser_BlogUserId",
                table: "BlogUser",
                column: "BlogUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogUser_BlogUser_BlogUserId",
                table: "BlogUser",
                column: "BlogUserId",
                principalTable: "BlogUser",
                principalColumn: "BlogUserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
