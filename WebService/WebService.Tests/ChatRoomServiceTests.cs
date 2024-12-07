using Domain.DTOs;
using Domain.Entities;
using Domain.Factories.Interfaces;
using Domain.Interfaces;
using Moq;
using Services;

namespace WebService.Tests
{
	public class ChatRoomServiceTests
	{
		// based on https://www.youtube.com/watch?v=NEtEmHgJBDQ 

		private readonly Mock<IChatRoomRepository> _chatRoomRepositoryMock;
		private readonly Mock<IChatRoomFactory> _chatRoomFactoryMock;
		private readonly ChatRoomService _chatRoomService;

		public ChatRoomServiceTests()
		{
			_chatRoomRepositoryMock = new Mock<IChatRoomRepository>();
			_chatRoomFactoryMock = new Mock<IChatRoomFactory>();
			_chatRoomService = new ChatRoomService(_chatRoomFactoryMock.Object, _chatRoomRepositoryMock.Object);
		}

		[Fact]
		public async Task CreateChatRoom_ShouldReturnChatRoom()
		{
			var userId = "user123";
			var chatRoomRequest = new CreateChatRoomRequest { Name = "Test Name", ChatRoomType = "Casual" };
			var createdChatRoom = new ChatRoom(userId, "Test Name", "Casual");

			_chatRoomFactoryMock
				.Setup(f => f.CreateChatRoom(chatRoomRequest, userId))
				.Returns(createdChatRoom);

			_chatRoomRepositoryMock
				.Setup(r => r.AddChatRoomAsync(It.IsAny<ChatRoom>()))
				.Returns(Task.CompletedTask);

			var result = await _chatRoomService.CreateChatRoom(chatRoomRequest, userId);

			Assert.NotNull(result);
			Assert.Equal("Test Name", result.Name);
			Assert.Equal("Casual", result.ChatRoomType);

			_chatRoomRepositoryMock.Verify(r => r.AddChatRoomAsync(It.IsAny<ChatRoom>()), Times.Once);
			_chatRoomRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
		}

		[Fact]
		public async Task GetChatRoomByUserId_ShouldReturnChatRooms()
		{
			var userId = "user123";
			var chatRooms = new List<ChatRoom>
		{
			new ChatRoom(Guid.NewGuid(), userId, "Chat1", "General"),
			new ChatRoom(Guid.NewGuid(), userId, "Chat2", "Topic")
		};


			_chatRoomRepositoryMock
				.Setup(r => r.GetChatRoomsByUserIdAsync(userId))
				.ReturnsAsync(chatRooms);

			var result = await _chatRoomService.GetChatRoomByUserId(userId);

			Assert.NotNull(result);
			Assert.Equal(2, result.Count());
			Assert.Contains(result, c => c.Name == "Chat1");
		}

		[Fact]
		public async Task UpdateChatRoom_ShouldReturnChatRoomst()
		{
			var chatRoomId = Guid.NewGuid();
			var userId = "user123";
			var updateChatRoomRequest = new UpdateChatRoomRequest { ChatRoomType = "Professional", Name = "Updated Name" };
			var existingChatRoom = new ChatRoom(userId, "Old Name", "Casual");


			_chatRoomRepositoryMock
				.Setup(r => r.GetChatRoomByIdAsync(chatRoomId))
				.ReturnsAsync(existingChatRoom);

			_chatRoomRepositoryMock
				.Setup(r => r.SaveAsync(existingChatRoom))
				.Returns(Task.CompletedTask);

			var result = await _chatRoomService.UpdateChatRoom(chatRoomId.ToString(), updateChatRoomRequest, userId);

			_chatRoomRepositoryMock.Verify(r => r.GetChatRoomByIdAsync(chatRoomId), Times.Once);
			_chatRoomRepositoryMock.Verify(r => r.SaveAsync(existingChatRoom), Times.Once);

			Assert.NotNull(result);
			Assert.Equal(updateChatRoomRequest.Name, result.Name);
			Assert.Equal(updateChatRoomRequest.ChatRoomType, result.ChatRoomType);
		}

		[Fact]
		public async Task DeleteChatRoomById_ShouldCallRepositoryDelete()
		{
			var chatRoomId = Guid.NewGuid();
			var userId = "user123";
			var chatRoom = new ChatRoom(chatRoomId, userId, "Chat 1", "Casual");

			_chatRoomRepositoryMock
				.Setup(r => r.GetChatRoomByIdAsync(chatRoomId))
				.ReturnsAsync(chatRoom);

			_chatRoomRepositoryMock
				.Setup(r => r.DeleteChatRoomAsync(chatRoomId))
				.Returns(Task.CompletedTask);

			await _chatRoomService.DeleteChatRoomById(chatRoomId.ToString(), userId);

			_chatRoomRepositoryMock.Verify(r => r.GetChatRoomByIdAsync(chatRoomId), Times.Once);
			_chatRoomRepositoryMock.Verify(r => r.DeleteChatRoomAsync(chatRoomId), Times.Once);
		}
	}
}
