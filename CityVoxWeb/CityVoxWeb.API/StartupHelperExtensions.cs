using CityVoxWeb.Data;
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

            return builder.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            return app;
        }
    }
}
