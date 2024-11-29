using Microsoft.Extensions.DependencyInjection;
using Services;
namespace API.Extensions;

public static class ApplicationServiceExtension
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services)
	{
		services.AddScoped<ChatRoomService>();
		services.AddControllers();
		return services;
	}
}
