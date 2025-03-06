namespace BasicSettings.Extensions.ProgramExtensions
{
    public static class SwaggerExtentions
    {
        public static void AddSwaggerSettings(this IServiceCollection services, Appsettings appsettings)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ForApi", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Enter your JWT token below.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT" // Bu formatni ko'rsatadi va Swagger avtomatik Bearer qo'shadi.
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        },
                        Array.Empty<string>()
                    },
                });

                if (appsettings.SwaggerSetting.PrefixIsEnable)
                    c.DocumentFilter<PathPrefixInsertDocumentFilter>(appsettings.SwaggerSetting.Prefix);
            });
        }
    }
}
