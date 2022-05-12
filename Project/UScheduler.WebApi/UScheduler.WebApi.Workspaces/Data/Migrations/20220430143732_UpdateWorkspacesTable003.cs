using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UScheduler.WebApi.Workspaces.Data.Migrations
{
    public partial class UpdateWorkspacesTable003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WorkspaceType",
                table: "Workspaces",
                newName: "WorkspaceTemplate");

            migrationBuilder.RenameColumn(
                name: "AccessType",
                table: "Workspaces",
                newName: "AccessLevel");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WorkspaceTemplate",
                table: "Workspaces",
                newName: "WorkspaceType");

            migrationBuilder.RenameColumn(
                name: "AccessLevel",
                table: "Workspaces",
                newName: "AccessType");
        }
    }
}
