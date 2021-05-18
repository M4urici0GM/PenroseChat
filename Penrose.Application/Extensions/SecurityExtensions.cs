using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Penrose.Application.Common;
using Penrose.Application.Interfaces;
using Penrose.Application.Options;

namespace Penrose.Application.Extensions
{
    public static class SecurityExtensions
    {
        public static void AddApplicationJwtBearer(this IServiceCollection services, IConfiguration configuration)
        {
            JwtTokenOptions tokenOptions = new JwtTokenOptions();
            new ConfigureFromConfigurationOptions<JwtTokenOptions>(configuration.GetSection(nameof(JwtTokenOptions)))
                .Configure(tokenOptions);

            IJwtSigningKey jwtSigningKey = new JwtSigningKey(tokenOptions);
            services.Configure<JwtTokenOptions>(configuration.GetSection(nameof(JwtTokenOptions)));
            services.AddSingleton(jwtSigningKey);


            services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidAudience = tokenOptions.Audience,
                    ValidIssuer = tokenOptions.Issuer,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = jwtSigningKey.SigningCredentials.Key,
                };
            });
        }
    }
}