using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Articles.Migrations
{
    public partial class likejunction2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostUserLike_BlogUser_BlogUserId",
                table: "PostUserLike");

            migrationBuilder.DropForeignKey(
                name: "FK_PostUserLike_Posts_PostId",
                table: "PostUserLike");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostUserLike",
                table: "PostUserLike");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostUserLikes",
                table: "PostUserLike",
                columns: new[] { "PostId", "BlogUserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PostUserLikes_BlogUser_BlogUserId",
                table: "PostUserLike",
                column: "BlogUserId",
                principalTable: "BlogUser",
                principalColumn: "BlogUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostUserLikes_Posts_PostId",
                table: "PostUserLike",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.RenameIndex(
                name: "IX_PostUserLike_PostId",
                table: "PostUserLike",
                newName: "IX_PostUserLikes_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_PostUserLike_BlogUserId",
                table: "PostUserLike",
                newName: "IX_PostUserLikes_BlogUserId");

            migrationBuilder.RenameTable(
                name: "PostUserLike",
                newName: "PostUserLikes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostUserLikes_BlogUser_BlogUserId",
                table: "PostUserLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_PostUserLikes_Posts_PostId",
                table: "PostUserLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostUserLikes",
                table: "PostUserLikes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostUserLike",
                table: "PostUserLikes",
                columns: new[] { "PostId", "BlogUserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PostUserLike_BlogUser_BlogUserId",
                table: "PostUserLikes",
                column: "BlogUserId",
                principalTable: "BlogUser",
                principalColumn: "BlogUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostUserLike_Posts_PostId",
                table: "PostUserLikes",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.RenameIndex(
                name: "IX_PostUserLikes_PostId",
                table: "PostUserLikes",
                newName: "IX_PostUserLike_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_PostUserLikes_BlogUserId",
                table: "PostUserLikes",
                newName: "IX_PostUserLike_BlogUserId");

            migrationBuilder.RenameTable(
                name: "PostUserLikes",
                newName: "PostUserLike");
        }
    }
}
