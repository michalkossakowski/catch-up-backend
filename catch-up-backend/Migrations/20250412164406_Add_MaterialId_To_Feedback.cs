using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace catch_up_backend.Migrations
{
    /// <inheritdoc />
    public partial class Add_MaterialId_To_Feedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaterialId",
                table: "Feedbacks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_MaterialId",
                table: "Feedbacks",
                column: "MaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Materials_MaterialId",
                table: "Feedbacks",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Materials_MaterialId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_MaterialId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "MaterialId",
                table: "Feedbacks");
        }
    }
}
