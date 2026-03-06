using Infra.CrossCutting.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infra.Ioc.Auth
{
    public static class JwtExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            // Configurar JwtSettings
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
            if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.SecretKey))
            {
                throw new InvalidOperationException("JwtSettings não configurado corretamente no appsettings.json");
            }

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            // Configurar autenticação JWT
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            // Configurar políticas de autorização por role
            services.AddAuthorization(options =>
            {
                // Política para passageiros
                options.AddPolicy("Passageiro", policy => policy.RequireRole("passageiro"));

                // Política para motoristas
                options.AddPolicy("Motorista", policy => policy.RequireRole("motorista"));

                // Política para administradores (se necessário no futuro)
                options.AddPolicy("Admin", policy => policy.RequireRole("admin"));

                // Política que permite passageiro ou motorista
                options.AddPolicy("PassageiroOuMotorista", policy => 
                    policy.RequireAssertion(context => 
                        context.User.IsInRole("passageiro") || 
                        context.User.IsInRole("motorista")));
            });

            return services;
        }
    }
}


