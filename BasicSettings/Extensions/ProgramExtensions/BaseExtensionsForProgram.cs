namespace BasicSettings.Extensions.ProgramExtensions
{
    public static class BaseExtensionsForProgram
    {
        public static void AddBasicSettings(this IServiceCollection services, WebApplicationBuilder builder)
        {
            var appsettings = builder.GetAppsettings();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAuthentication(appsettings);
            services.AddCorsSettins(appsettings);
            services.AddService(appsettings);
            services.AddSwaggerSettings(appsettings);
            services.AddCache(appsettings);
            services.AddClient();
        }
    }
}
