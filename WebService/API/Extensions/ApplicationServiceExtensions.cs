using Domain.Factories.Interfaces;
using Domain.Factories;
using Domain.Interfaces;
using Services;
using Services.Interfaces;
using Data.Repositories;
using Domain.Strategies.Interfaces;
namespace API.Extensions;

public static class ApplicationServiceExtension
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services)
	{
		services.AddScoped<IChatRoomService, ChatRoomService>();
		services.AddScoped<IMessageService, MessageService>();

		services.AddScoped<IChatRoomFactory, ChatRoomFactory>();
		services.AddScoped<IChatStrategyFactory, ChatStrategyFactory>();

		services.AddScoped<IChatRoomRepository, ChatRoomRepository>();
		services.AddScoped<IMessageRepository, MessageRepository>();

		services.AddHttpClient<IChatResponseGenerator, ResponseGeneratorService>();

		services.AddControllers();
		return services;
	}
}
