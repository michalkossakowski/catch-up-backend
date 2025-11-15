using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace catch_up_backend.Migrations
{
    /// <inheritdoc />
    public partial class schooling_update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolingParts_Files_IconFileId",
                table: "SchoolingParts");

            migrationBuilder.DropForeignKey(
                name: "FK_Schoolings_Files_IconFileId",
                table: "Schoolings");

            migrationBuilder.DropTable(
                name: "MaterialsSchoolingParts");

            migrationBuilder.DropTable(
                name: "SchoolingUserParts");

            migrationBuilder.DropTable(
                name: "SchoolingsUsers");

            migrationBuilder.DropIndex(
                name: "IX_Schoolings_IconFileId",
                table: "Schoolings");

            migrationBuilder.DropIndex(
                name: "IX_SchoolingParts_IconFileId",
                table: "SchoolingParts");

            migrationBuilder.DropColumn(
                name: "IconFileId",
                table: "Schoolings");

            migrationBuilder.DropColumn(
                name: "IconFileId",
                table: "SchoolingParts");

            migrationBuilder.AddColumn<int>(
                name: "MaterialsId",
                table: "SchoolingParts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaterialsId",
                table: "SchoolingParts");

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

            migrationBuilder.CreateTable(
                name: "MaterialsSchoolingParts",
                columns: table => new
                {
                    MaterialsId = table.Column<int>(type: "int", nullable: false),
                    SchoolingPartId = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialsSchoolingParts", x => new { x.MaterialsId, x.SchoolingPartId });
                    table.ForeignKey(
                        name: "FK_MaterialsSchoolingParts_Materials_MaterialsId",
                        column: x => x.MaterialsId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialsSchoolingParts_SchoolingParts_SchoolingPartId",
                        column: x => x.SchoolingPartId,
                        principalTable: "SchoolingParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolingsUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewbieId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchoolingId = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolingsUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolingsUsers_Schoolings_SchoolingId",
                        column: x => x.SchoolingId,
                        principalTable: "Schoolings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SchoolingsUsers_Users_NewbieId",
                        column: x => x.NewbieId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

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
                    table.PrimaryKey("PK_SchoolingUserParts", x => new { x.SchoolingUserId, x.SchoolingPartId });
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
                name: "IX_Schoolings_IconFileId",
                table: "Schoolings",
                column: "IconFileId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolingParts_IconFileId",
                table: "SchoolingParts",
                column: "IconFileId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialsSchoolingParts_SchoolingPartId",
                table: "MaterialsSchoolingParts",
                column: "SchoolingPartId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolingsUsers_NewbieId",
                table: "SchoolingsUsers",
                column: "NewbieId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolingsUsers_SchoolingId",
                table: "SchoolingsUsers",
                column: "SchoolingId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolingUserParts_SchoolingPartId",
                table: "SchoolingUserParts",
                column: "SchoolingPartId");

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
    }
}
