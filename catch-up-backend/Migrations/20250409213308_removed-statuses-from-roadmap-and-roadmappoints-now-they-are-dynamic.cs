using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace catch_up_backend.Migrations
{
    /// <inheritdoc />
    public partial class removedstatusesfromroadmapandroadmappointsnowtheyaredynamic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "RoadMaps");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "RoadMapPoints");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
