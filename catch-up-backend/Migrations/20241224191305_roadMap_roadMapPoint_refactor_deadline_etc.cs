using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace catch_up_backend.Migrations
{
    /// <inheritdoc />
    public partial class roadMap_roadMapPoint_refactor_deadline_etc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFinished",
                table: "RoadMaps");

            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "RoadMapPoints");

            migrationBuilder.RenameColumn(
                name: "FinalizationDate",
                table: "RoadMapPoints",
                newName: "FinishDate");

            migrationBuilder.RenameColumn(
                name: "AssignmentDate",
                table: "RoadMapPoints",
                newName: "StartDate");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "RoadMaps",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "RoadMaps");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "RoadMapPoints",
                newName: "AssignmentDate");

            migrationBuilder.RenameColumn(
                name: "FinishDate",
                table: "RoadMapPoints",
                newName: "FinalizationDate");

            migrationBuilder.AddColumn<bool>(
                name: "IsFinished",
                table: "RoadMaps",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Deadline",
                table: "RoadMapPoints",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
