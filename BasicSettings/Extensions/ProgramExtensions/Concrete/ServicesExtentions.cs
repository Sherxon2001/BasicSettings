﻿using BasicSettings.Services.Auth.Concrete;
using BasicSettings.Services.Helper.Concrete;

namespace BasicSettings.Extensions.ProgramExtensions
{
    public static class ServicesExtentions
    {
        public static void AddService(this IServiceCollection services, AppSettings appsettings)
        {
            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IHelperService, HelperService>();
            services.AddScoped<ILoggerService, LoggerService>();
        }
    }
}
