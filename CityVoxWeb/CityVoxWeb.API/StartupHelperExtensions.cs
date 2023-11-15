using CityVoxWeb.Data;
using CityVoxWeb.DTOs.Issues.Emergencies;
using CityVoxWeb.DTOs.Issues.InfIssues;
using CityVoxWeb.DTOs.Issues.Reports;
using CityVoxWeb.Services.Interfaces;
using CityVoxWeb.Services.Issue_Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace CityVoxWeb.API
{
    public static class StartupHelperExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
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

            //Adding services
            //Issue manipulating services
            builder.Services.AddScoped<IGenericIssuesService<CreateReportDto, ExportReportDto, UpdateReportDto>, ReportsService>();
            builder.Services.AddScoped<IGenericIssuesService<CreateEmergencyDto, ExportEmergencyDto, UpdateEmergencyDto>, EmergenciesService>();
            builder.Services.AddScoped<IGenericIssuesService<CreateInfIssueDto, ExportInfIssueDto, UpdateInfIssueDto>, InfrastructureIssuesService>();
           
            
            return builder.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
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
