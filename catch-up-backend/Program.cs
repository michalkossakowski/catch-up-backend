using catch_up_backend.Database;
using catch_up_backend.Interfaces;
using catch_up_backend.Services;
using catch_up_backend.FileManagers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


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
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.ASCII.GetBytes(builder.Configuration["Jwt:AccessTokenSecret"])
                        ),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            //Services
            builder.Services.AddScoped<IFaqService, FaqService>();
            builder.Services.AddSingleton<FileStorageFactory>();
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddScoped<IMaterialService, MaterialService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<INewbieMentorService, NewbieMentorService>();
            //CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });
            // ----------- Custom Section End -----------


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            //----------- Custom Section Start -----------
            app.UseCors("AllowAllOrigins");
            // ----------- Custom Section End -----------

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
