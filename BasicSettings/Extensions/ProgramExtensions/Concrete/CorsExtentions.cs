namespace BasicSettings.Extensions.ProgramExtensions
{
    public static class CorsExtentions
    {
        public static void AddCorsSettins(this IServiceCollection services, Appsettings appsettings)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecific", builder =>
                {
                    builder.WithOrigins(appsettings.AllowedOrigins.ToArray())
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
        }
    }
}
