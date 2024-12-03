using Domain.Factories.Interfaces;
using Domain.Factories;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.Interfaces;
using WebService.Repositories;
namespace API.Extensions;

public static class ApplicationServiceExtension
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services)
	{
		services.AddScoped<IChatRoomService, ChatRoomService>();
		services.AddScoped<IChatRoomService, ChatRoomService>();
		services.AddScoped<IChatRoomFactory, ChatRoomFactory>(); // Factory
		services.AddScoped<IChatRoomRepository, ChatRoomRepository>(); // Repository
		services.AddControllers();
		return services;
	}
}
