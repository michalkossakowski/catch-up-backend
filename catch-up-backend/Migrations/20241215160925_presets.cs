using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace catch_up_backend.Migrations
{
    /// <inheritdoc />
    public partial class presets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TasksPresets_Presets_PresetId",
                table: "TasksPresets");

            migrationBuilder.DropForeignKey(
                name: "FK_TasksPresets_TaskContents_TaskContentId",
                table: "TasksPresets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TasksPresets",
                table: "TasksPresets");

            migrationBuilder.RenameTable(
                name: "TasksPresets",
                newName: "TaskPresets");

            migrationBuilder.RenameIndex(
                name: "IX_TasksPresets_TaskContentId",
                table: "TaskPresets",
                newName: "IX_TaskPresets_TaskContentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskPresets",
                table: "TaskPresets",
                columns: new[] { "PresetId", "TaskContentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TaskPresets_Presets_PresetId",
                table: "TaskPresets",
                column: "PresetId",
                principalTable: "Presets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskPresets_TaskContents_TaskContentId",
                table: "TaskPresets",
                column: "TaskContentId",
                principalTable: "TaskContents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskPresets_Presets_PresetId",
                table: "TaskPresets");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskPresets_TaskContents_TaskContentId",
                table: "TaskPresets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskPresets",
                table: "TaskPresets");

            migrationBuilder.RenameTable(
                name: "TaskPresets",
                newName: "TasksPresets");

            migrationBuilder.RenameIndex(
                name: "IX_TaskPresets_TaskContentId",
                table: "TasksPresets",
                newName: "IX_TasksPresets_TaskContentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TasksPresets",
                table: "TasksPresets",
                columns: new[] { "PresetId", "TaskContentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TasksPresets_Presets_PresetId",
                table: "TasksPresets",
                column: "PresetId",
                principalTable: "Presets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TasksPresets_TaskContents_TaskContentId",
                table: "TasksPresets",
                column: "TaskContentId",
                principalTable: "TaskContents",
                principalColumn: "Id");
        }
    }
}
