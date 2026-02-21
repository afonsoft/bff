using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AuthenticationInjection
    {
        public static IServiceCollection ConfigAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            string? authority = configuration.GetValue<string>("AuthenticationConfiguration:Authority");
            string? validAudiencesStr = configuration.GetValue<string>("AuthenticationConfiguration:ValidAudiences");
            string[] validAudiences = validAudiencesStr?.Split(',') ?? Array.Empty<string>();

            if (string.IsNullOrEmpty(authority))
            {
                throw new ArgumentException("AuthenticationConfiguration:Authority is required");
            }

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.ConfigurationManager = new CustomConfigurationManager(authority);
                x.Authority = authority;
                x.UseSecurityTokenValidators = true;
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.IncludeErrorDetails = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidAudiences = validAudiences
                };
            });

            return services;
        }
    }
}