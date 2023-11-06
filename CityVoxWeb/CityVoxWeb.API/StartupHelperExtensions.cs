using CityVoxWeb.Data;
using Microsoft.EntityFrameworkCore;

namespace CityVoxWeb.API
{
    public static class StartupHelperExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
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
