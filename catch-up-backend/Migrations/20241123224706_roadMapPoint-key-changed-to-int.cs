using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace catch_up_backend.Migrations
{
    public partial class roadMapPointkeychangedtoint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_RoadMapPoints_RoadMapPointId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoadMapPoints",
                table: "RoadMapPoints");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "RoadMapPoints");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "RoadMapPoints",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoadMapPoints",
                table: "RoadMapPoints",
                column: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "RoadMapPointId",
                table: "Tasks",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_RoadMapPoints_RoadMapPointId",
                table: "Tasks",
                column: "RoadMapPointId",
                principalTable: "RoadMapPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_RoadMapPoints_RoadMapPointId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoadMapPoints",
                table: "RoadMapPoints");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "RoadMapPoints");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "RoadMapPoints",
                type: "nvarchar(450)",
                nullable: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoadMapPoints",
                table: "RoadMapPoints",
                column: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "RoadMapPointId",
                table: "Tasks",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_RoadMapPoints_RoadMapPointId",
                table: "Tasks",
                column: "RoadMapPointId",
                principalTable: "RoadMapPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
