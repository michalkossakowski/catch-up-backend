using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace catch_up_backend.Migrations
{
    /// <inheritdoc />
    public partial class roadmapforeighkeyrepaired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RoadMaps_CreatorId",
                table: "RoadMaps",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoadMaps_Users_CreatorId",
                table: "RoadMaps",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoadMaps_Users_CreatorId",
                table: "RoadMaps");

            migrationBuilder.DropIndex(
                name: "IX_RoadMaps_CreatorId",
                table: "RoadMaps");
        }
    }
}
