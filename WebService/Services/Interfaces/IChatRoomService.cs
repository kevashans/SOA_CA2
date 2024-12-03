﻿using Domain.DTOs;
using Domain.Entities;


namespace Services.Interfaces;
public interface IChatRoomService
{
	Task<ChatRoom> CreateChatRoom(CreateChatRoomRequest chatroomRequest, string userId);
	Task<ChatRoom> UpdateChatRoom(UpdateChatRoomRequest updateChatRoomRequest, string userId);

}
