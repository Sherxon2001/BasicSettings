namespace BasicSettings.Extensions.ProgramExtensions
{
    public static class CorsExtentions
    {
        public static void AddCorsSettins(this IServiceCollection services, AppSettings appsettings)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(appsettings.AllowCorsSpecific, builder =>
                {
                    builder.WithOrigins(appsettings.AllowedOrigins.ToArray())
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
        }
    }
}
