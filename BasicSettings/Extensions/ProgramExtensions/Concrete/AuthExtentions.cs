namespace BasicSettings.Extensions.ProgramExtensions
{
    public static class AuthExtentions
    {
        public static void AddAuthentication(this IServiceCollection services, AppSettings appsettings)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appsettings.AuthSettings.SecretKey)),
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidIssuer = appsettings.AuthSettings.ValidIssuer,
                    ValidAudience = appsettings.AuthSettings.ValidAudience,
                    ValidateAudience = true
                };
            });
        }
    }
}
