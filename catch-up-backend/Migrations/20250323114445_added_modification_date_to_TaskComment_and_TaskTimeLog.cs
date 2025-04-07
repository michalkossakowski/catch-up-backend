using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace catch_up_backend.Migrations
{
    /// <inheritdoc />
    public partial class added_modification_date_to_TaskComment_and_TaskTimeLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "TaskTimeLog",
                newName: "CreationDate");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "TaskComments",
                newName: "CreationDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationDate",
                table: "TaskTimeLog",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationDate",
                table: "TaskComments",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModificationDate",
                table: "TaskTimeLog");

            migrationBuilder.DropColumn(
                name: "ModificationDate",
                table: "TaskComments");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "TaskTimeLog",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "TaskComments",
                newName: "Date");
        }
    }
}
