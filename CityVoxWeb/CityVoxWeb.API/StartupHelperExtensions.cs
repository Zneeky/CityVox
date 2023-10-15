namespace CityVoxWeb.API
{
    public static class StartupHelperExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {

            return builder.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            return app;
        }
    }
}
