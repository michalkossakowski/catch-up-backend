using catch_up_backend.Database;
using catch_up_backend.Interfaces;
using catch_up_backend.Services;
using catch_up_backend.FileManagers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using catch_up_backend.Interfaces.RepositoryInterfaces;
using catch_up_backend.Repositories;
using catch_up_backend.Services.Interfaces;
using catch_up_backend.Models;
using System.Security.Cryptography;
using catch_up_backend.Hubs;
using System.Text.Json;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;


namespace catch_up_backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //----------- Custom Section Start -----------
            //Database
            var connectionString = builder.Configuration.GetConnectionString("catchUpConnectionString") ?? throw new InvalidOperationException("Connection string 'catchUpConnectionString' not found.");
            builder.Services.AddDbContext<CatchUpDbContext>(options => options.UseSqlServer(connectionString));

            //Authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)   // this tells .NET to use JWT  bearer authentication as the default one
                .AddJwtBearer(options => {                                               // comapnySettings fot the JWT bearer authentication
                        options.TokenValidationParameters = new TokenValidationParameters    // this object contains all of the rules for validating
                    {
                        ValidateIssuerSigningKey = true,                                 // this tells .NET to validate the issuer signing key
                        IssuerSigningKey = new SymmetricSecurityKey(                     // this gets the secret key from the secret user config and converts it to a byte array
                            Encoding.ASCII.GetBytes(builder.Configuration["Jwt:AccessTokenSecret"])
                        ),
                        ValidateIssuer = true,                                           // tells .NET to validate who created the token 
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],               // sets the issuer to the one from the user secrets (Issuer)
                        ValidateAudience = true,                                         // tells .NET to check who the token is intended for
                        ValidAudience = builder.Configuration["Jwt:Audience"],           // sets the audience to the one from the user secrets (Audience)
                        ClockSkew = TimeSpan.Zero                                        // sets a time window for the token to be valid (In production change to ~10sec), because there might be a situation when token is still valid on the client but not on the server.
                    };

                    options.Events = new JwtBearerEvents                                 // reading the token from the cookie
                    {
                        OnMessageReceived = context => {                                 // when a request comes in, this function will run
                            context.Token = context.Request.Cookies["accessToken"];      // get the token from the "accessToken" cookie insteaf of the Authorization header
                            return Task.CompletedTask;
                        }
                    };
                });

            // Services
            builder.Services.AddScoped<IFaqService, FaqService>();
            builder.Services.AddScoped<IBadgeService, BadgeService>();
            builder.Services.AddSingleton<FileStorageFactory>();
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddScoped<IMaterialService, MaterialService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<INewbieMentorService, NewbieMentorService>();
            builder.Services.AddScoped<ITaskContentService, TaskContentService>();
            builder.Services.AddScoped<ITaskService, TaskService>();
            builder.Services.AddScoped<IRoadMapService, RoadMapService>();
            builder.Services.AddScoped<IRoadMapPointService, RoadMapPointService>();
            builder.Services.AddScoped<IFeedbackService, FeedbackService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<ISchoolingService, SchoolingService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ISchoolingPartService, SchoolingPartService>();
            builder.Services.AddScoped<IPresetService, PresetService>();
            builder.Services.AddScoped<ITaskPresetService, TaskPresetService>();
            builder.Services.AddScoped<IEventService, EventService>(); 
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<INotificationHubService, NotificationHubService>();
            builder.Services.AddScoped<IAIService, AIService>();
            builder.Services.AddScoped<IFirebaseService, FirebaseService>();

            // Repositories
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            builder.Services.AddScoped<ICompanySettingsService, CompanySettingsService>();
            builder.Services.AddScoped<ICompanyCityService, CompanyCityService>();

            //CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", builder =>
                {
                    builder.SetIsOriginAllowed(_ => true) 
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                });
            });


            // SignalR
            builder.Services.AddSignalR().AddJsonProtocol(options => {
                options.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });

            // Firebase
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("Config/catch-up-onboarding-firebase.json")
            });

            //Logging
            builder.Logging.AddConsole();
            builder.Logging.AddFilter("System.Net.Http", LogLevel.Debug);

            // ----------- Custom Section End -----------

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()){
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();

            //----------- Custom Section Start -----------
            // CORS
            app.UseCors("AllowAllOrigins");

            // SignalR
            app.MapHub<NotificationHub>("/notificationHub").RequireCors("AllowAllOrigins");

            // create a default user if Users table is empty
            void EnsureUserExists(IServiceProvider serviceProvider)
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<CatchUpDbContext>();

                    string HashPassword(string password)
                    {
                        using (var sha256 = SHA256.Create())
                        {
                            var bytes = Encoding.UTF8.GetBytes(password);
                            var hash = sha256.ComputeHash(bytes);
                            return Convert.ToBase64String(hash);
                        }
                    }

                    var hashedPassword = HashPassword("Admin");

                    if (!context.Users.Any())
                    {
                        var adminUser = new UserModel(
                            name: "Admin",
                            surname: "Admin",
                            email: "admin@admin.com",
                            password: hashedPassword,
                            type: "Admin",
                            position: "Admin"
                        );

                        context.Users.Add(adminUser);
                        context.SaveChanges();
                    }
                }
            }

            EnsureUserExists(app.Services);
            // ----------- Custom Section End -----------

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
