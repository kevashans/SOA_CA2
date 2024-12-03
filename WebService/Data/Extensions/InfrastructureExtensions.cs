using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Data.Auth;

namespace Data.Extensions;

public static class InfrastructureServiceExtensions
{
	public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
	{
		// Database Configuration
		services.AddDbContext<ApplicationDbContext>(options =>
		{
			var connectionString = configuration.GetConnectionString("DbConnection");
			options.UseSqlServer(connectionString);
		});

		// Identity Configuration
		services.AddIdentityCore<User>(options =>
		{
			options.Password.RequireDigit = true;
			options.Password.RequiredLength = 6;
			options.Password.RequireNonAlphanumeric = false;
		})
		.AddEntityFrameworkStores<ApplicationDbContext>()
		.AddApiEndpoints();

		// Authentication Configuration
		services.AddAuthentication()
			.AddBearerToken(IdentityConstants.BearerScheme);

		// Authorization Configuration
		services.AddAuthorization();

		return services;
	}
}
