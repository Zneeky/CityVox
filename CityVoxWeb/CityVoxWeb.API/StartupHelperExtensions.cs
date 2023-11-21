using CityVoxWeb.API.Middleware;
using CityVoxWeb.Data;
using CityVoxWeb.Data.Models.UserEntities;
using CityVoxWeb.DTOs.Issues.Emergencies;
using CityVoxWeb.DTOs.Issues.InfIssues;
using CityVoxWeb.DTOs.Issues.Reports;
using CityVoxWeb.DTOs.User;
using CityVoxWeb.Mapper;
using CityVoxWeb.Services.Interfaces;
using CityVoxWeb.Services.Issue_Services;
using CityVoxWeb.Services.Token_Services;
using CityVoxWeb.Services.User_Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

namespace CityVoxWeb.API
{
    public static class StartupHelperExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            //Swagger API documentation service
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            //Identity configuration
            builder.Services
                         .AddIdentity<ApplicationUser, IdentityRole<Guid>>
                         (options =>
                         {
                             options.Password.RequireDigit = true;
                             options.Password.RequiredLength = 6;
                             options.Password.RequireLowercase = true;
                             options.Password.RequireUppercase = true;
                             options.Password.RequireNonAlphanumeric = false;

                             options.SignIn.RequireConfirmedEmail = true;
                         })
                         .AddEntityFrameworkStores<CityVoxDbContext>()
                         .AddDefaultTokenProviders();

            
            //JWT Token configuration
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        //ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])),
                        RoleClaimType = ClaimTypes.Role,
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            var result = System.Text.Json.JsonSerializer.Serialize(new { message = "Unauthorized" });
                            return context.Response.WriteAsync(result);
                        },
                    };
                });

            builder.Services.AddControllers(configure =>
            {
                configure.ReturnHttpNotAcceptable = true;

            })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                })
                .AddXmlDataContractSerializerFormatters();

            //CORS Policy configuration
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });

            // Database connection configuration
            builder.Services.AddDbContext<CityVoxDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
            });

            //AutoMapper Dependency Injection
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);

            //Adding services
            //Issue manipulating services
            builder.Services.AddScoped<IGenericIssuesService<CreateReportDto, ExportReportDto, UpdateReportDto>, ReportsService>();
            builder.Services.AddScoped<IGenericIssuesService<CreateEmergencyDto, ExportEmergencyDto, UpdateEmergencyDto>, EmergenciesService>();
            builder.Services.AddScoped<IGenericIssuesService<CreateInfIssueDto, ExportInfIssueDto, UpdateInfIssueDto>, InfrastructureIssuesService>();
            builder.Services.AddScoped<IJwtUtils, JWTService>();
            builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            builder.Services.AddScoped<IEmailService,EmailService>();
            builder.Services.AddScoped<IUsersService, UsersService>();

            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            builder.Services.Configure<SMTPConfigModel>(builder.Configuration.GetSection("SMTPConfig"));
            
            return builder.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            app.UseMiddleware<JwtTokenCookieMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseResponseCaching();

            // app.UseRouting();

            app.UseCors("AllowAll");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

     
            return app;
        }
    }
}
