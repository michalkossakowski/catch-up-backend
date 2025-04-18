using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace catch_up_backend.Migrations
{
    /// <inheritdoc />
    public partial class changingFKOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SchoolingUserParts",
                table: "SchoolingUserParts");

            migrationBuilder.DropIndex(
                name: "IX_SchoolingUserParts_SchoolingUserId",
                table: "SchoolingUserParts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SchoolingUserParts",
                table: "SchoolingUserParts",
                columns: new[] { "SchoolingUserId", "SchoolingPartId" });

            migrationBuilder.CreateIndex(
                name: "IX_SchoolingUserParts_SchoolingPartId",
                table: "SchoolingUserParts",
                column: "SchoolingPartId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SchoolingUserParts",
                table: "SchoolingUserParts");

            migrationBuilder.DropIndex(
                name: "IX_SchoolingUserParts_SchoolingPartId",
                table: "SchoolingUserParts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SchoolingUserParts",
                table: "SchoolingUserParts",
                columns: new[] { "SchoolingPartId", "SchoolingUserId" });

            migrationBuilder.CreateIndex(
                name: "IX_SchoolingUserParts_SchoolingUserId",
                table: "SchoolingUserParts",
                column: "SchoolingUserId");
        }
    }
}
