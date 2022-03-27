using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace UScheduler.WebApi.Workspaces.Data.Migrations
{
    public partial class InitialCreate001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Workspaces",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Owner = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccessType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ColabUsersIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkspaceType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workspaces", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Workspaces");
        }
    }
}
