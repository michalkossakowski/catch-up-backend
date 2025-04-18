using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace catch_up_backend.Migrations
{
    /// <inheritdoc />
    public partial class Schooling_Rework : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SchoolingsUsers",
                table: "SchoolingsUsers");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Schoolings",
                newName: "ShortDescription");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "SchoolingParts",
                newName: "Title");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "SchoolingsUsers",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Schoolings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "IconFileId",
                table: "Schoolings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IconFileId",
                table: "SchoolingParts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "SchoolingParts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "SchoolingParts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SchoolingsUsers",
                table: "SchoolingsUsers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "SchoolingUserParts",
                columns: table => new
                {
                    SchoolingUserId = table.Column<int>(type: "int", nullable: false),
                    SchoolingPartId = table.Column<int>(type: "int", nullable: false),
                    IsDone = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolingUserParts", x => new { x.SchoolingPartId, x.SchoolingUserId });
                    table.ForeignKey(
                        name: "FK_SchoolingUserParts_SchoolingParts_SchoolingPartId",
                        column: x => x.SchoolingPartId,
                        principalTable: "SchoolingParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolingUserParts_SchoolingsUsers_SchoolingUserId",
                        column: x => x.SchoolingUserId,
                        principalTable: "SchoolingsUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SchoolingsUsers_NewbieId",
                table: "SchoolingsUsers",
                column: "NewbieId");

            migrationBuilder.CreateIndex(
                name: "IX_Schoolings_IconFileId",
                table: "Schoolings",
                column: "IconFileId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolingParts_IconFileId",
                table: "SchoolingParts",
                column: "IconFileId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolingUserParts_SchoolingUserId",
                table: "SchoolingUserParts",
                column: "SchoolingUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolingParts_Files_IconFileId",
                table: "SchoolingParts",
                column: "IconFileId",
                principalTable: "Files",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Schoolings_Files_IconFileId",
                table: "Schoolings",
                column: "IconFileId",
                principalTable: "Files",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolingParts_Files_IconFileId",
                table: "SchoolingParts");

            migrationBuilder.DropForeignKey(
                name: "FK_Schoolings_Files_IconFileId",
                table: "Schoolings");

            migrationBuilder.DropTable(
                name: "SchoolingUserParts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SchoolingsUsers",
                table: "SchoolingsUsers");

            migrationBuilder.DropIndex(
                name: "IX_SchoolingsUsers_NewbieId",
                table: "SchoolingsUsers");

            migrationBuilder.DropIndex(
                name: "IX_Schoolings_IconFileId",
                table: "Schoolings");

            migrationBuilder.DropIndex(
                name: "IX_SchoolingParts_IconFileId",
                table: "SchoolingParts");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "SchoolingsUsers");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Schoolings");

            migrationBuilder.DropColumn(
                name: "IconFileId",
                table: "Schoolings");

            migrationBuilder.DropColumn(
                name: "IconFileId",
                table: "SchoolingParts");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "SchoolingParts");

            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "SchoolingParts");

            migrationBuilder.RenameColumn(
                name: "ShortDescription",
                table: "Schoolings",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "SchoolingParts",
                newName: "Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SchoolingsUsers",
                table: "SchoolingsUsers",
                columns: new[] { "NewbieId", "SchoolingId" });
        }
    }
}
