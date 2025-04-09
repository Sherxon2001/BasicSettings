namespace BasicSettings.Extensions.ProgramExtensions
{
    public static class GetAppsettingsExtentions
    {
        public static Appsettings GetAppsettings(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<Appsettings>(builder.Configuration);
            builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<Appsettings>>().Value);
            var appsettings = new Appsettings();
            return appsettings;
        }
    }
}
