using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace catch_up_backend.Migrations
{
    /// <inheritdoc />
    public partial class roadMapandroadMapPointmodelsupdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoadMapPoints_RoadMaps_RoadmapId",
                table: "RoadMapPoints");

            migrationBuilder.RenameColumn(
                name: "RoadmapId",
                table: "RoadMapPoints",
                newName: "RoadMapId");

            migrationBuilder.RenameIndex(
                name: "IX_RoadMapPoints_RoadmapId",
                table: "RoadMapPoints",
                newName: "IX_RoadMapPoints_RoadMapId");

            migrationBuilder.AddColumn<DateTime>(
                name: "FinishDate",
                table: "RoadMaps",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "RoadMaps",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "RoadMaps",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "RoadMapPoints",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_RoadMapPoints_RoadMaps_RoadMapId",
                table: "RoadMapPoints",
                column: "RoadMapId",
                principalTable: "RoadMaps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoadMapPoints_RoadMaps_RoadMapId",
                table: "RoadMapPoints");

            migrationBuilder.DropColumn(
                name: "FinishDate",
                table: "RoadMaps");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "RoadMaps");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "RoadMaps");

            migrationBuilder.RenameColumn(
                name: "RoadMapId",
                table: "RoadMapPoints",
                newName: "RoadmapId");

            migrationBuilder.RenameIndex(
                name: "IX_RoadMapPoints_RoadMapId",
                table: "RoadMapPoints",
                newName: "IX_RoadMapPoints_RoadmapId");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "RoadMapPoints",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_RoadMapPoints_RoadMaps_RoadmapId",
                table: "RoadMapPoints",
                column: "RoadmapId",
                principalTable: "RoadMaps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
