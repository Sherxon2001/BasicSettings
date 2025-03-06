namespace BasicSettings.Extensions.ProgramExtensions
{
    public static class ServicesExtentions
    {
        public static void AddService(this IServiceCollection services, Appsettings appsettings)
        {
            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IHelperService, HelperService>();
            services.AddScoped<ILoggerService, LoggerService>();
            services.AddSingleton<RestClient>();
            services.AddSingleton<IntegrationClient>();
        }
    }
}
