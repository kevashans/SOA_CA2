using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Message
{
	public Message(Guid chatRoomId, string messageType, string content, DateTime createdAt)
	{
		ChatRoomId = chatRoomId;
		MessageType = messageType;
		Content = content;
		CreatedAt = createdAt;
	}

	public Message(Guid messageId, Guid chatRoomId, string messageType, string content, DateTime createdAt)
	{
		MessageId = messageId;
		ChatRoomId = chatRoomId;
		MessageType = messageType;
		Content = content;
		CreatedAt = createdAt;
	}

	/// <summary>
	/// Id of the generated message (Primary Key)
	/// </summary>
	public Guid MessageId { get; set; }

	/// <summary>
	/// The Id of the chatroom the message belong in (Foreign Key)
	/// </summary>
	public Guid ChatRoomId { get; set; }

	/// <summary>
	/// MessageType can either be input or output
	/// </summary>
	public string MessageType { get; set; } = null!;

	/// <summary>
	/// Content of the message
	/// </summary>
	public string Content { get; set; } = null!;

	/// <summary>
	/// When the message is created
	/// </summary>
	public DateTime CreatedAt { get; set; }
}
