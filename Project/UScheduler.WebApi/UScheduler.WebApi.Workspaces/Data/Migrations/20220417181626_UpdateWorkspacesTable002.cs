using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace UScheduler.WebApi.Workspaces.Data.Migrations
{
    public partial class UpdateWorkspacesTable002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ColabUsersIds",
                table: "Workspaces",
                newName: "UpdatedBy");

            migrationBuilder.AlterColumn<string>(
                name: "Owner",
                table: "Workspaces",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "Colabs",
                table: "Workspaces",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Workspaces",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Workspaces",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Workspaces",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Colabs",
                table: "Workspaces");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Workspaces");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Workspaces");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Workspaces");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Workspaces",
                newName: "ColabUsersIds");

            migrationBuilder.AlterColumn<Guid>(
                name: "Owner",
                table: "Workspaces",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
