﻿using Domain.Factories.Interfaces;
using Domain.Factories;
using Domain.Interfaces;
using Services;
using Services.Interfaces;
using Data.Repositories;
namespace API.Extensions;

public static class ApplicationServiceExtension
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services)
	{
		services.AddScoped<IChatRoomService, ChatRoomService>();
		services.AddScoped<IChatRoomService, ChatRoomService>();
		services.AddScoped<IChatRoomFactory, ChatRoomFactory>(); 
		services.AddScoped<IChatRoomRepository, ChatRoomRepository>(); 
		services.AddControllers();
		return services;
	}
}
