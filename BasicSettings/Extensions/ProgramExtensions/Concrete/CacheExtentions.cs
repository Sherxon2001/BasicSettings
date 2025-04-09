namespace BasicSettings.Extensions.ProgramExtensions
{
    public static class CacheExtentions
    {
        public static void AddCache(this IServiceCollection services, Appsettings appsettings)
        {
            services.AddMemoryCache();
        }
    }
}
