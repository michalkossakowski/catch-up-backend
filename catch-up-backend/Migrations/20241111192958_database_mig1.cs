using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace catch_up_backend.Migrations
{
    /// <inheritdoc />
    public partial class database_mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "UsersNotifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "TasksPresets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "TaskContents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "SchoolingsUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Schoolings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "SchoolingParts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "RoadMaps",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "RoadMapPoints",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Presets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Points",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "NewbiesMentors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "MentorsBadges",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "MaterialsSchoolingParts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Materials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Grades",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "FileInMaterials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Feedbacks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Faqs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "EmployeeCards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Badges",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "UsersNotifications");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "State",
                table: "TasksPresets");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "State",
                table: "TaskContents");

            migrationBuilder.DropColumn(
                name: "State",
                table: "SchoolingsUsers");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Schoolings");

            migrationBuilder.DropColumn(
                name: "State",
                table: "SchoolingParts");

            migrationBuilder.DropColumn(
                name: "State",
                table: "RoadMaps");

            migrationBuilder.DropColumn(
                name: "State",
                table: "RoadMapPoints");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Presets");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Points");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "State",
                table: "NewbiesMentors");

            migrationBuilder.DropColumn(
                name: "State",
                table: "MentorsBadges");

            migrationBuilder.DropColumn(
                name: "State",
                table: "MaterialsSchoolingParts");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "State",
                table: "FileInMaterials");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Faqs");

            migrationBuilder.DropColumn(
                name: "State",
                table: "EmployeeCards");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Badges");
        }
    }
}
