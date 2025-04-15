using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace catch_up_backend.Migrations
{
    /// <inheritdoc />
    public partial class newhandlingforstatusandfinishdateroadmapandroadmappoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Progress",
                table: "RoadMaps",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "RoadMaps",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "RoadMapPoints",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Progress",
                table: "RoadMaps");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "RoadMaps");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "RoadMapPoints");
        }
    }
}
