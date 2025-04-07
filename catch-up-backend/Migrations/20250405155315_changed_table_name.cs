using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace catch_up_backend.Migrations
{
    /// <inheritdoc />
    public partial class changed_table_name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskTimeLog_Tasks_TaskId",
                table: "TaskTimeLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskTimeLog",
                table: "TaskTimeLog");

            migrationBuilder.RenameTable(
                name: "TaskTimeLog",
                newName: "TaskTimeLogs");

            migrationBuilder.RenameIndex(
                name: "IX_TaskTimeLog_TaskId",
                table: "TaskTimeLogs",
                newName: "IX_TaskTimeLogs_TaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskTimeLogs",
                table: "TaskTimeLogs",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "EventReceivers",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "int", nullable: false),
                    ReceiverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventReceivers", x => new { x.EventId, x.ReceiverId });
                    table.ForeignKey(
                        name: "FK_EventReceivers_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EventReceivers_Users_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_OwnerId",
                table: "Events",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_EventReceivers_ReceiverId",
                table: "EventReceivers",
                column: "ReceiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Users_OwnerId",
                table: "Events",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTimeLogs_Tasks_TaskId",
                table: "TaskTimeLogs",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Users_OwnerId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskTimeLogs_Tasks_TaskId",
                table: "TaskTimeLogs");

            migrationBuilder.DropTable(
                name: "EventReceivers");

            migrationBuilder.DropIndex(
                name: "IX_Events_OwnerId",
                table: "Events");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskTimeLogs",
                table: "TaskTimeLogs");

            migrationBuilder.RenameTable(
                name: "TaskTimeLogs",
                newName: "TaskTimeLog");

            migrationBuilder.RenameIndex(
                name: "IX_TaskTimeLogs_TaskId",
                table: "TaskTimeLog",
                newName: "IX_TaskTimeLog_TaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskTimeLog",
                table: "TaskTimeLog",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTimeLog_Tasks_TaskId",
                table: "TaskTimeLog",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id");
        }
    }
}
