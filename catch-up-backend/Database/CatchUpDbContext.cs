using catch_up_backend.Enums;
using catch_up_backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace catch_up_backend.Database
{
    public class CatchUpDbContext : DbContext
    {
        public CatchUpDbContext(DbContextOptions<CatchUpDbContext> options) : base(options) { }

        public DbSet<BadgeModel> Badges { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<EmployeeCardModel> EmployeeCards { get; set; }
        public DbSet<FaqModel> Faqs { get; set; }
        public DbSet<FeedbackModel> Feedbacks { get; set; }
        public DbSet<FileModel> Files { get; set; }
        public DbSet<FileInMaterial> FileInMaterials { get; set; }
        public DbSet<GradeModel> Grades { get; set; }
        public DbSet<MaterialsSchoolingPartModel> MaterialsSchoolingParts { get; set; }
        public DbSet<MaterialsModel> Materials { get; set; }
        public DbSet<MentorBadgeModel> MentorsBadges { get; set; }
        public DbSet<NewbieMentorModel> NewbiesMentors { get; set; }
        public DbSet<NotificationModel> Notifications { get; set; }
        public DbSet<PointsModel> Points { get; set; }
        public DbSet<PresetModel> Presets { get; set; }
        public DbSet<RoadMapModel> RoadMaps { get; set; }
        public DbSet<RoadMapPointModel> RoadMapPoints { get; set; }
        public DbSet<SchoolingModel> Schoolings { get; set; }
        public DbSet<SchoolingPartModel> SchoolingParts { get; set; }
        public DbSet<SchoolingUserModel> SchoolingsUsers { get; set; }
        public DbSet<TaskContentModel> TaskContents { get; set; }
        public DbSet<TaskPresetModel> TaskPresets { get; set; }
        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<RefreshTokenModel> RefreshTokens { get; set; }
        public DbSet<FirebaseTokenModel> FirebaseTokens { get; set; }
        public DbSet<UserNotificationModel> UsersNotifications { get; set; }
        public DbSet<SettingModel> CompanySettings { get; set; }
        public DbSet<CompanyCity> CompanyCities { get; set; }
        public DbSet<EventModel> Events { get; set; }
        public DbSet<TaskCommentModel> TaskComments { get; set; }
        public DbSet<TaskTimeLogModel> TaskTimeLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //FaqModel One To Many
            modelBuilder.Entity<FaqModel>()
                .HasOne<MaterialsModel>()
                .WithMany()
                .HasForeignKey(x => x.MaterialId);

            //FeedbackModel One To Many
            modelBuilder.Entity<FeedbackModel>()
                .HasOne<UserModel>()
                .WithMany()
                .HasForeignKey(x => x.SenderId)
                .OnDelete(DeleteBehavior.NoAction);
            //FeedbackModel One To Many
            modelBuilder.Entity<FeedbackModel>()
                .HasOne<UserModel>()
                .WithMany()
                .HasForeignKey(x => x.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);

            /*//FileModel One To Many
            modelBuilder.Entity<FileModel>()
                .HasOne<MaterialsModel>()
                .WithMany()
                .HasForeignKey(x => x.MaterialsId);*/

            //FileInMaterial Many To Many
            modelBuilder.Entity<FileInMaterial>()
                .HasKey(x => new { x.FileId, x.MaterialId });
            modelBuilder.Entity<FileInMaterial>()
                .HasOne<FileModel>()
                .WithMany()
                .HasForeignKey(x => x.FileId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<FileInMaterial>()
                .HasOne<MaterialsModel>()
                .WithMany()
                .HasForeignKey(x => x.MaterialId)
                .OnDelete(DeleteBehavior.NoAction);

            //GradeModel One To Many
            modelBuilder.Entity<GradeModel>()
                .HasOne<UserModel>()
                .WithMany()
                .HasForeignKey(x => x.RatedId)
                .OnDelete(DeleteBehavior.NoAction);
            //GradeModel One To Many
            modelBuilder.Entity<GradeModel>()
                .HasOne<UserModel>()
                .WithMany()
                .HasForeignKey(x => x.EvaluatorId)
                .OnDelete(DeleteBehavior.NoAction);

            //MaterialsSchoolingPartModel Many To Many
            modelBuilder.Entity<MaterialsSchoolingPartModel>()
                .HasKey(x => new { x.MaterialsId, x.SchoolingPartId });
            modelBuilder.Entity<MaterialsSchoolingPartModel>()
                .HasOne<MaterialsModel>()
                .WithMany()
                .HasForeignKey(x => x.MaterialsId);
            modelBuilder.Entity<MaterialsSchoolingPartModel>()
                .HasOne<SchoolingPartModel>()
                .WithMany()
                .HasForeignKey(x => x.SchoolingPartId);

            //MentorBadgeModel Many To Many
            modelBuilder.Entity<MentorBadgeModel>()
                .HasKey(x => new { x.MentorId, x.BadgeId });
            modelBuilder.Entity<MentorBadgeModel>()
                .HasOne<UserModel>()
                .WithMany()
                .HasForeignKey(x => x.MentorId);
            modelBuilder.Entity<MentorBadgeModel>()
                .HasOne<BadgeModel>()
                .WithMany()
                .HasForeignKey(x => x.BadgeId);

            //NewbieMentorModel Many To Many
            modelBuilder.Entity<NewbieMentorModel>()
                .HasKey(x => new { x.NewbieId, x.MentorId });
            modelBuilder.Entity<NewbieMentorModel>()
                .HasOne<UserModel>()
                .WithMany()
                .HasForeignKey(x => x.NewbieId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<NewbieMentorModel>()
                .HasOne<UserModel>()
                .WithMany()
                .HasForeignKey(x => x.MentorId)
                .OnDelete(DeleteBehavior.NoAction);

            //NotificationModel One To Many
            modelBuilder.Entity<NotificationModel>()
                .HasOne<UserModel>()
                .WithMany()
                .HasForeignKey(x => x.SenderId);

            //PointsModel One To Many
            modelBuilder.Entity<PointsModel>()
                .HasOne<UserModel>()
                .WithMany()
                .HasForeignKey(x => x.MentorId);

            //PresetModel One To Many
            modelBuilder.Entity<PresetModel>()
                .HasOne<UserModel>()
                .WithMany()
                .HasForeignKey(x => x.CreatorId);

            //RoadMapModel One To Many
            modelBuilder.Entity<RoadMapModel>()
                .HasOne<UserModel>()
                .WithMany()
                .HasForeignKey(x => x.NewbieId)
                .OnDelete(DeleteBehavior.NoAction);
           
            //RoadMapModel One To Many
            modelBuilder.Entity<RoadMapModel>()
                .HasOne<UserModel>()
                .WithMany()
                .HasForeignKey(x => x.CreatorId)
                .OnDelete(DeleteBehavior.NoAction);

            //RoadMapPointModel One To Many
            modelBuilder.Entity<RoadMapPointModel>()
                .HasOne<RoadMapModel>()
                .WithMany()
                .HasForeignKey(x => x.RoadMapId)
                .OnDelete(DeleteBehavior.NoAction); ;

            //SchoolingModel One To Many
            modelBuilder.Entity<SchoolingModel>()
                .HasOne<UserModel>()
                .WithMany()
                .HasForeignKey(x => x.CreatorId);
            //SchoolingModel One To Many
            modelBuilder.Entity<SchoolingModel>()
                .HasOne<CategoryModel>()
                .WithMany()
                .HasForeignKey(x => x.CategoryId);

            //SchoolingPartModel One To Many
            modelBuilder.Entity<SchoolingPartModel>()
                .HasOne<SchoolingModel>()
                .WithMany()
                .HasForeignKey(x => x.SchoolingId);

            //SchoolingUserModel Many To Many
            modelBuilder.Entity<SchoolingUserModel>()
                .HasKey(x => new { x.NewbieId, x.SchoolingId });
            modelBuilder.Entity<SchoolingUserModel>()
                .HasOne<UserModel>()
                .WithMany()
                .HasForeignKey(x => x.NewbieId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<SchoolingUserModel>()
                .HasOne<SchoolingModel>()
                .WithMany()
                .HasForeignKey(x => x.SchoolingId)
                .OnDelete(DeleteBehavior.NoAction);

            //TaskContentModel One To Many
            modelBuilder.Entity<TaskContentModel>()
                .HasOne<UserModel>()
                .WithMany()
                .HasForeignKey(x => x.CreatorId);
            //TaskContentModel One To Many
            modelBuilder.Entity<TaskContentModel>()
                .HasOne<CategoryModel>()
                .WithMany()
                .HasForeignKey(x => x.CategoryId);
            //TaskContentModel One To Many
            modelBuilder.Entity<TaskContentModel>()
                .HasOne<MaterialsModel>()
                .WithMany()
                .HasForeignKey(x => x.MaterialsId);

            //TaskModel One To Many
            modelBuilder.Entity<TaskModel>()
                .HasOne<UserModel>()
                .WithMany()
                .HasForeignKey(x => x.NewbieId)
                .OnDelete(DeleteBehavior.NoAction);
            //TaskModel One To Many
            modelBuilder.Entity<TaskModel>()
                .HasOne<TaskContentModel>()
                .WithMany()
                .HasForeignKey(x => x.TaskContentId)
                .OnDelete(DeleteBehavior.NoAction);
            //TaskModel One To Many
            modelBuilder.Entity<TaskModel>()
                .HasOne<RoadMapPointModel>()
                .WithMany()
                .HasForeignKey(x => x.RoadMapPointId)
                .OnDelete(DeleteBehavior.NoAction);

            //SchoolingUserModel Many To Many
            modelBuilder.Entity<TaskPresetModel>()
                .HasKey(x => new { x.PresetId, x.TaskContentId });
            modelBuilder.Entity<TaskPresetModel>()
                .HasOne<PresetModel>()
                .WithMany()
                .HasForeignKey(x => x.PresetId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<TaskPresetModel>()
                .HasOne<TaskContentModel>()
                .WithMany()
                .HasForeignKey(x => x.TaskContentId)
                .OnDelete(DeleteBehavior.NoAction);

            //UserNotificationModel Many To Many
            modelBuilder.Entity<UserNotificationModel>()
                .HasKey(x => new { x.NotificationId, x.ReceiverId });
            modelBuilder.Entity<UserNotificationModel>()
                .HasOne<NotificationModel>()
                .WithMany()
                .HasForeignKey(x => x.NotificationId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<UserNotificationModel>()
                .HasOne<UserModel>()
                .WithMany()
                .HasForeignKey(x => x.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<EventModel>()
            .HasOne<UserModel>()
            .WithMany()
            .HasForeignKey(x => x.OwnerId)
            .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<EventModel>()
            .HasMany<UserModel>()
            .WithMany("Events")
            .UsingEntity<Dictionary<string, object>>(
            "EventReceivers",
            j => j
            .HasOne<UserModel>()
            .WithMany()
            .HasForeignKey("ReceiverId")
            .OnDelete(DeleteBehavior.NoAction),
            j => j
            .HasOne<EventModel>()
            .WithMany()
            .HasForeignKey("EventId")
            .OnDelete(DeleteBehavior.NoAction)
            );

            //TaskCommentModel One To Many
            modelBuilder.Entity<TaskCommentModel>()
                .HasOne<TaskModel>()
                .WithMany()
                .HasForeignKey(x => x.TaskId)
                .OnDelete(DeleteBehavior.NoAction);
            //TaskCommentModel One To Many
            modelBuilder.Entity<TaskCommentModel>()
                .HasOne<UserModel>()
                .WithMany()
                .HasForeignKey(x => x.CreatorId)
                .OnDelete(DeleteBehavior.NoAction);
            //TaskCommentModel One To Many
            modelBuilder.Entity<TaskCommentModel>()
                .HasOne<MaterialsModel>()
                .WithMany()
                .HasForeignKey(x => x.MaterialId)
                .OnDelete(DeleteBehavior.NoAction);

            //TaskTimeLogModel One To Many
            modelBuilder.Entity<TaskTimeLogModel>()
                .HasOne<TaskModel>()
                .WithMany()
                .HasForeignKey(x => x.TaskId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserModel>()
                .Property(u => u.Counters)
                .HasColumnType("nvarchar(max)")
                .HasConversion(
                    v => v == null ? null : JsonSerializer.Serialize(v, new JsonSerializerOptions
                    {
                        Converters = { new JsonStringEnumConverter() }
                    }),
                    v => string.IsNullOrEmpty(v) ?
                          catch_up_backend.Models.UserModel.InitializeCounters() :
                          JsonSerializer.Deserialize<Dictionary<BadgeTypeCountEnum, int>>(
                              v,
                              new JsonSerializerOptions
                              {
                                  Converters = { new JsonStringEnumConverter() }
                              }
                          )
                )
                .Metadata.SetValueComparer(
                    new ValueComparer<Dictionary<BadgeTypeCountEnum, int>>(
                        (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => c == null ? null : c.ToDictionary(entry => entry.Key, entry => entry.Value)
                    )
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}