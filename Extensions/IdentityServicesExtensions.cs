using jwtAuthWebAPI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace jwtAuthWebAPI.Extensions
{
    public static class IdentityServicesExtensions
    {
        public  static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdentityDataContext>(options =>
           {
               options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

           });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<IdentityDataContext>();
            return services;    
        }
    }
}
