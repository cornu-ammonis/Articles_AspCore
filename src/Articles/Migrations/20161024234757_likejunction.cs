using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Articles.Migrations
{
    public partial class likejunction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostUserLike",
                columns: table => new
                {
                    PostId = table.Column<int>(nullable: false),
                    BlogUserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostUserLike", x => new { x.PostId, x.BlogUserId });
                    table.ForeignKey(
                        name: "FK_PostUserLike_BlogUser_BlogUserId",
                        column: x => x.BlogUserId,
                        principalTable: "BlogUser",
                        principalColumn: "BlogUserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostUserLike_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostUserLike_BlogUserId",
                table: "PostUserLike",
                column: "BlogUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PostUserLike_PostId",
                table: "PostUserLike",
                column: "PostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostUserLike");
        }
    }
}
