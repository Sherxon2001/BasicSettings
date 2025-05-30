using BasicSettings.Services.Client.Concrete;

namespace BasicSettings.Extensions.ProgramExtensions
{
    public static class AddClientExtentions
    {
        public static void AddClient(this IServiceCollection services)
        {
            services.AddSingleton<RestClient>();
            services.AddScoped<IntegrationClient>();
        }
    }
}
