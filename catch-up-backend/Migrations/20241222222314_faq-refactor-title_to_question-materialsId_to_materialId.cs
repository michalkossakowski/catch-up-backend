using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace catch_up_backend.Migrations
{
    /// <inheritdoc />
    public partial class faqrefactortitle_to_questionmaterialsId_to_materialId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Faqs_Materials_MaterialsId",
                table: "Faqs");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Faqs",
                newName: "Question");

            migrationBuilder.RenameColumn(
                name: "MaterialsId",
                table: "Faqs",
                newName: "MaterialId");

            migrationBuilder.RenameIndex(
                name: "IX_Faqs_MaterialsId",
                table: "Faqs",
                newName: "IX_Faqs_MaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_Faqs_Materials_MaterialId",
                table: "Faqs",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Faqs_Materials_MaterialId",
                table: "Faqs");

            migrationBuilder.RenameColumn(
                name: "Question",
                table: "Faqs",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "MaterialId",
                table: "Faqs",
                newName: "MaterialsId");

            migrationBuilder.RenameIndex(
                name: "IX_Faqs_MaterialId",
                table: "Faqs",
                newName: "IX_Faqs_MaterialsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Faqs_Materials_MaterialsId",
                table: "Faqs",
                column: "MaterialsId",
                principalTable: "Materials",
                principalColumn: "Id");
        }
    }
}
