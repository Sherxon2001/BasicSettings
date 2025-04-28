namespace BasicSettings.Extensions.ProgramExtensions.Concrete
{
    public static class AddConfigureDbtExtentions
    {
        public static void AddConfigureDbServices(this IServiceCollection services, AppSettings appsettings)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(appsettings.ConnectionStrings.DefaultConnection);
            });
        }
    }
}
