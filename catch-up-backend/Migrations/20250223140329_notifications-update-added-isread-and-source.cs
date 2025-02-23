using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace catch_up_backend.Migrations
{
    /// <inheritdoc />
    public partial class notificationsupdateaddedisreadandsource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LinkedContent",
                table: "Notifications",
                newName: "Source");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UsersNotifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "UsersNotifications",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "UsersNotifications");

            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "UsersNotifications");

            migrationBuilder.RenameColumn(
                name: "Source",
                table: "Notifications",
                newName: "LinkedContent");
        }
    }
}
