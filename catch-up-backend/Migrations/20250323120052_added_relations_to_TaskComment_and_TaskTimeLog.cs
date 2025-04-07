using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace catch_up_backend.Migrations
{
    /// <inheritdoc />
    public partial class added_relations_to_TaskComment_and_TaskTimeLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TaskTimeLog_TaskId",
                table: "TaskTimeLog",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskComments_CreatorId",
                table: "TaskComments",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskComments_MaterialId",
                table: "TaskComments",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskComments_TaskId",
                table: "TaskComments",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskComments_Materials_MaterialId",
                table: "TaskComments",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskComments_Tasks_TaskId",
                table: "TaskComments",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskComments_Users_CreatorId",
                table: "TaskComments",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTimeLog_Tasks_TaskId",
                table: "TaskTimeLog",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskComments_Materials_MaterialId",
                table: "TaskComments");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskComments_Tasks_TaskId",
                table: "TaskComments");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskComments_Users_CreatorId",
                table: "TaskComments");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskTimeLog_Tasks_TaskId",
                table: "TaskTimeLog");

            migrationBuilder.DropIndex(
                name: "IX_TaskTimeLog_TaskId",
                table: "TaskTimeLog");

            migrationBuilder.DropIndex(
                name: "IX_TaskComments_CreatorId",
                table: "TaskComments");

            migrationBuilder.DropIndex(
                name: "IX_TaskComments_MaterialId",
                table: "TaskComments");

            migrationBuilder.DropIndex(
                name: "IX_TaskComments_TaskId",
                table: "TaskComments");
        }
    }
}
