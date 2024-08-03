using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PlaceRentalApp.Core.Repositories;
using PlaceRentalApp.Infrasctructure.Auth;
using PlaceRentalApp.Infrasctructure.Persistence;
using PlaceRentalApp.Infrasctructure.Repositories;
using System.Text;

namespace PlaceRentalApp.Infrasctructure
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
                                                           IConfiguration configuration)
        {
            services.AddData(configuration)
                    .AddRepositories()
                    .AddAuth(configuration);

            return services;
        }

        public static IServiceCollection AddData(this IServiceCollection services,
                                                 IConfiguration configuration)
        {
            // builder.Services.AddDbContext<PlaceRentalDbContext>(opt =>
            //     opt.UseInMemoryDatabase("PlaceRentalDbInMemory"));

            var connectionString = configuration.GetConnectionString("PlaceRentalCs");
            services.AddDbContext<PlaceRentalDbContext>(options => 
                options.UseSqlServer(connectionString));

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IPlaceRepository, PlaceRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            
            return services;
        }

        private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = configuration["Jwt:Issuer"],
                            ValidAudience = configuration["Jwt:Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey
                                (Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                        };
                    });

            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
