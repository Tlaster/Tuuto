using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tuuto.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Draft",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccessToken = table.Column<string>(nullable: true),
                    AccountId = table.Column<int>(nullable: false),
                    Domain = table.Column<string>(nullable: true),
                    ErrorMessage = table.Column<string>(nullable: true),
                    InReplyToId = table.Column<int>(nullable: false),
                    Sensitive = table.Column<bool>(nullable: false),
                    SpoilerText = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Visibility = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Draft", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MediaData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DraftModelId = table.Column<int>(nullable: true),
                    SavedFile = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaData_Draft_DraftModelId",
                        column: x => x.DraftModelId,
                        principalTable: "Draft",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MediaData_DraftModelId",
                table: "MediaData",
                column: "DraftModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MediaData");

            migrationBuilder.DropTable(
                name: "Draft");
        }
    }
}
