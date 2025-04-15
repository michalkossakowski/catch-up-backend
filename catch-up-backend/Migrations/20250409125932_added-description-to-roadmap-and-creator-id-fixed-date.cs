using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace catch_up_backend.Migrations
{
    /// <inheritdoc />
    public partial class addeddescriptiontoroadmapandcreatoridfixeddate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "RoadMaps",
                newName: "AssignDate");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "RoadMaps",
                newName: "Title");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "RoadMaps",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "RoadMaps",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "RoadMapPoints",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "RoadMaps");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "RoadMaps");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "RoadMaps",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "AssignDate",
                table: "RoadMaps",
                newName: "StartDate");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "RoadMapPoints",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
