namespace BasicSettings.Extensions.ProgramExtensions
{
    public static class BaseExtensionsForProgram
    {
        public static void AddBasicSettings(this WebApplicationBuilder builder)
        {
            var appsettings = builder.Configuration.Get<AppSettings>();

            builder.AddAppsettingsCofigure();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddConfigureDbServices(appsettings);
            builder.Services.AddAuthentication(appsettings);
            builder.Services.AddCorsSettins(appsettings);
            builder.Services.AddService(appsettings);
            builder.Services.AddSwaggerSettings(appsettings);
            builder.Services.AddCache(appsettings);
            builder.Services.AddClient();
            builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
        }

        public static void AddBasicMiddlewareSettings(this WebApplication app, WebApplicationBuilder builder)
        {
            var appsettings = builder.Configuration.Get<AppSettings>();

            if (appsettings.SwaggerSetting.IsSwaggerEnable)
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors(appsettings.AllowCorsSpecific);

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
        }
    }
}