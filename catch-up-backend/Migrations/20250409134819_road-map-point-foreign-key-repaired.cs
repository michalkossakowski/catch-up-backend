using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace catch_up_backend.Migrations
{
    /// <inheritdoc />
    public partial class roadmappointforeignkeyrepaired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoadMapPoints_RoadMaps_RoadMapId",
                table: "RoadMapPoints");

            migrationBuilder.AddForeignKey(
                name: "FK_RoadMapPoints_RoadMaps_RoadMapId",
                table: "RoadMapPoints",
                column: "RoadMapId",
                principalTable: "RoadMaps",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoadMapPoints_RoadMaps_RoadMapId",
                table: "RoadMapPoints");

            migrationBuilder.AddForeignKey(
                name: "FK_RoadMapPoints_RoadMaps_RoadMapId",
                table: "RoadMapPoints",
                column: "RoadMapId",
                principalTable: "RoadMaps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
