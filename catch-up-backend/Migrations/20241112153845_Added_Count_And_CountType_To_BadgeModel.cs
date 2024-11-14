using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace catch_up_backend.Migrations
{
    /// <inheritdoc />
    public partial class Added_Count_And_CountType_To_BadgeModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Badges",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountType",
                table: "Badges",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "Badges");

            migrationBuilder.DropColumn(
                name: "CountType",
                table: "Badges");
        }
    }
}
