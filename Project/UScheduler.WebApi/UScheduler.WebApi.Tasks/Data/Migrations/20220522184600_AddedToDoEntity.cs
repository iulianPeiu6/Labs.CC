using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UScheduler.WebApi.Tasks.Data.Migrations
{
    public partial class AddedToDoEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ToDos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Completed = table.Column<bool>(type: "bit", nullable: false),
                    TaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToDos_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ToDos_TaskId",
                table: "ToDos",
                column: "TaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ToDos");
        }
    }
}
