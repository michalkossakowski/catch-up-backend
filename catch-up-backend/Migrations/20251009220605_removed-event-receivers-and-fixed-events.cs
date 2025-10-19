using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace catch_up_backend.Migrations
{
    /// <inheritdoc />
    public partial class removedeventreceiversandfixedevents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventReceivers");

            migrationBuilder.RenameColumn(
                name: "ReceiverIds",
                table: "Events",
                newName: "TargetUserType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TargetUserType",
                table: "Events",
                newName: "ReceiverIds");

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
                name: "IX_EventReceivers_ReceiverId",
                table: "EventReceivers",
                column: "ReceiverId");
        }
    }
}
