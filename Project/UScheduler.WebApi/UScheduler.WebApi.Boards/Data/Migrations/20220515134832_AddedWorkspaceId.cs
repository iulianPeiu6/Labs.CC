using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace UScheduler.WebApi.Boards.Data.Migrations
{
    public partial class AddedWorkspaceId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WorkspaceId",
                table: "Boards",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkspaceId",
                table: "Boards");
        }
    }
}
