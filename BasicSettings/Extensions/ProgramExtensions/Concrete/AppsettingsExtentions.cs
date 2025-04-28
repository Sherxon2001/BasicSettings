namespace BasicSettings.Extensions.ProgramExtensions.Concrete
{
    public static class AppsettingsExtentions
    {
        public static void AddAppsettingsCofigure(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<AppSettings>(builder.Configuration);
            builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<AppSettings>>().Value);
        }
    }
}
