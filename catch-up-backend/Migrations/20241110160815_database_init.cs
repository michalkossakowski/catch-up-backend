using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace catch_up_backend.Migrations
{
    /// <inheritdoc />
    public partial class database_init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Badges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IconSource = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Badges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TelephoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdditionalInformation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvatarSource = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeCards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Faqs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaterialsId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faqs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Faqs_Materials_MaterialsId",
                        column: x => x.MaterialsId,
                        principalTable: "Materials",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FileInMaterials",
                columns: table => new
                {
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    FileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileInMaterials", x => new { x.FileId, x.MaterialId });
                    table.ForeignKey(
                        name: "FK_FileInMaterials_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FileInMaterials_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Origin = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Users_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Feedbacks_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RatedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EvaluatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grades_Users_EvaluatorId",
                        column: x => x.EvaluatorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Grades_Users_RatedId",
                        column: x => x.RatedId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MentorsBadges",
                columns: table => new
                {
                    MentorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BadgeId = table.Column<int>(type: "int", nullable: false),
                    AchievedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MentorsBadges", x => new { x.MentorId, x.BadgeId });
                    table.ForeignKey(
                        name: "FK_MentorsBadges_Badges_BadgeId",
                        column: x => x.BadgeId,
                        principalTable: "Badges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MentorsBadges_Users_MentorId",
                        column: x => x.MentorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NewbiesMentors",
                columns: table => new
                {
                    NewbieId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MentorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewbiesMentors", x => new { x.NewbieId, x.MentorId });
                    table.ForeignKey(
                        name: "FK_NewbiesMentors_Users_MentorId",
                        column: x => x.MentorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NewbiesMentors_Users_NewbieId",
                        column: x => x.NewbieId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SendDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LinkedContent = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Points",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MentorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Points", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Points_Users_MentorId",
                        column: x => x.MentorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Presets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Presets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Presets_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoadMaps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewbieId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsFinished = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoadMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoadMaps_Users_NewbieId",
                        column: x => x.NewbieId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Schoolings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schoolings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schoolings_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Schoolings_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskContents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    MaterialsId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskContents_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskContents_Materials_MaterialsId",
                        column: x => x.MaterialsId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskContents_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersNotifications",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    ReceiverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersNotifications", x => new { x.NotificationId, x.ReceiverId });
                    table.ForeignKey(
                        name: "FK_UsersNotifications_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notifications",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UsersNotifications_Users_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RoadMapPoints",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoadmapId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssignmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinalizationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deadline = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoadMapPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoadMapPoints_RoadMaps_RoadmapId",
                        column: x => x.RoadmapId,
                        principalTable: "RoadMaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolingParts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolingId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolingParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolingParts_Schoolings_SchoolingId",
                        column: x => x.SchoolingId,
                        principalTable: "Schoolings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolingsUsers",
                columns: table => new
                {
                    NewbieId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchoolingId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolingsUsers", x => new { x.NewbieId, x.SchoolingId });
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
                name: "TasksPresets",
                columns: table => new
                {
                    PresetId = table.Column<int>(type: "int", nullable: false),
                    TaskContentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TasksPresets", x => new { x.PresetId, x.TaskContentId });
                    table.ForeignKey(
                        name: "FK_TasksPresets_Presets_PresetId",
                        column: x => x.PresetId,
                        principalTable: "Presets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TasksPresets_TaskContents_TaskContentId",
                        column: x => x.TaskContentId,
                        principalTable: "TaskContents",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewbieId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaskContentId = table.Column<int>(type: "int", nullable: false),
                    RoadMapPointId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssignmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinalizationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deadline = table.Column<int>(type: "int", nullable: false),
                    SpendTime = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Rate = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_RoadMapPoints_RoadMapPointId",
                        column: x => x.RoadMapPointId,
                        principalTable: "RoadMapPoints",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tasks_TaskContents_TaskContentId",
                        column: x => x.TaskContentId,
                        principalTable: "TaskContents",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tasks_Users_NewbieId",
                        column: x => x.NewbieId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MaterialsSchoolingParts",
                columns: table => new
                {
                    MaterialsId = table.Column<int>(type: "int", nullable: false),
                    SchoolingPartId = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_Faqs_MaterialsId",
                table: "Faqs",
                column: "MaterialsId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_ReceiverId",
                table: "Feedbacks",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_SenderId",
                table: "Feedbacks",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_FileInMaterials_MaterialId",
                table: "FileInMaterials",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_EvaluatorId",
                table: "Grades",
                column: "EvaluatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_RatedId",
                table: "Grades",
                column: "RatedId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialsSchoolingParts_SchoolingPartId",
                table: "MaterialsSchoolingParts",
                column: "SchoolingPartId");

            migrationBuilder.CreateIndex(
                name: "IX_MentorsBadges_BadgeId",
                table: "MentorsBadges",
                column: "BadgeId");

            migrationBuilder.CreateIndex(
                name: "IX_NewbiesMentors_MentorId",
                table: "NewbiesMentors",
                column: "MentorId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_SenderId",
                table: "Notifications",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Points_MentorId",
                table: "Points",
                column: "MentorId");

            migrationBuilder.CreateIndex(
                name: "IX_Presets_CreatorId",
                table: "Presets",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_RoadMapPoints_RoadmapId",
                table: "RoadMapPoints",
                column: "RoadmapId");

            migrationBuilder.CreateIndex(
                name: "IX_RoadMaps_NewbieId",
                table: "RoadMaps",
                column: "NewbieId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolingParts_SchoolingId",
                table: "SchoolingParts",
                column: "SchoolingId");

            migrationBuilder.CreateIndex(
                name: "IX_Schoolings_CategoryId",
                table: "Schoolings",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Schoolings_CreatorId",
                table: "Schoolings",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolingsUsers_SchoolingId",
                table: "SchoolingsUsers",
                column: "SchoolingId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskContents_CategoryId",
                table: "TaskContents",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskContents_CreatorId",
                table: "TaskContents",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskContents_MaterialsId",
                table: "TaskContents",
                column: "MaterialsId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_NewbieId",
                table: "Tasks",
                column: "NewbieId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_RoadMapPointId",
                table: "Tasks",
                column: "RoadMapPointId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskContentId",
                table: "Tasks",
                column: "TaskContentId");

            migrationBuilder.CreateIndex(
                name: "IX_TasksPresets_TaskContentId",
                table: "TasksPresets",
                column: "TaskContentId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersNotifications_ReceiverId",
                table: "UsersNotifications",
                column: "ReceiverId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeCards");

            migrationBuilder.DropTable(
                name: "Faqs");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "FileInMaterials");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "MaterialsSchoolingParts");

            migrationBuilder.DropTable(
                name: "MentorsBadges");

            migrationBuilder.DropTable(
                name: "NewbiesMentors");

            migrationBuilder.DropTable(
                name: "Points");

            migrationBuilder.DropTable(
                name: "SchoolingsUsers");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "TasksPresets");

            migrationBuilder.DropTable(
                name: "UsersNotifications");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "SchoolingParts");

            migrationBuilder.DropTable(
                name: "Badges");

            migrationBuilder.DropTable(
                name: "RoadMapPoints");

            migrationBuilder.DropTable(
                name: "Presets");

            migrationBuilder.DropTable(
                name: "TaskContents");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Schoolings");

            migrationBuilder.DropTable(
                name: "RoadMaps");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
