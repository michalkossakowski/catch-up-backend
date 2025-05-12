using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace catch_up_backend.Migrations
{
    /// <inheritdoc />
    public partial class change_icon_type_for_badge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconSource",
                table: "Badges");

            migrationBuilder.AddColumn<int>(
                name: "IconId",
                table: "Badges",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconId",
                table: "Badges");

            migrationBuilder.AddColumn<string>(
                name: "IconSource",
                table: "Badges",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
