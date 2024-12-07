import * as React from 'react';
import { TextField, MenuItem, Select, FormControl, InputLabel, Button } from '@mui/material';
import { createChatRoom, deleteChatRoom } from '../api/actionRequests';
import { getChatRooms } from '../api/dataRequests';

export type chatRoomTypes = 'professional' | 'casual' | 'pirate';

export interface chartRoom  {
  chatRoomId;
  chatRoomType;
  name;
  userId;
}

export const AddChatroomPage = ({
  addChatRoom,
  chatRooms,
  setChatRooms
}: {
    addChatRoom: (name: string, chatRoomType: chatRoomTypes, chatRoomId: string) => void;
    chatRooms: any[];
    setChatRooms: any;
}) => {
  const [name, setName] = React.useState('');
  const [chatRoomType, setChatRoomType] = React.useState<chatRoomTypes | ''>('');

  const chatRoomTypesList = ['professional', 'casual', 'pirate'];

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();
    const accessToken = localStorage.getItem('accessToken');
  
    try {
      // Call the API to create a new chatroom
      await createChatRoom(name, chatRoomType, accessToken);
  
      // Fetch all chatrooms to get the latest details
      const response = await getChatRooms(accessToken);
      const responseData = await response.json(); // Parse the JSON response
      const chatRooms = responseData.chatRooms as chartRoom[]; // Access the `chatRooms` array
      
      // Get the latest room with the matching name and type
      const latestChatRoom = chatRooms.find(
        (room) =>
          room.name.trim().toLowerCase() === name.trim().toLowerCase() &&
          room.chatRoomType.trim().toLowerCase() === chatRoomType.trim().toLowerCase()
      );
  
      if (latestChatRoom) {
        // Add the latest chatroom to the navigation
        addChatRoom(latestChatRoom.name, latestChatRoom.chatRoomType, latestChatRoom.chatRoomId);
  
        // Clear the form
        setName('');
        setChatRoomType('');
      } else {
        console.error('Failed to fetch the latest chatroom.');
      }
    } catch (error) {
      console.error('Error creating or fetching chatroom:', error);
    }
  };

  const handleDelete = async (chatRoomId: string) => {
    const accessToken = localStorage.getItem('accessToken');
    try {
      // Call API to delete the chatroom
      await deleteChatRoom(chatRoomId, accessToken);
  
      // Update state to remove the deleted chatroom
      setChatRooms((prevRooms) => prevRooms.filter((room) => room.segment !== chatRoomId));
    } catch (error) {
      console.error('Error deleting chatroom:', error);
    }
  };
  
  const handleEdit = async (chatRoomId: string, updatedName: string, updatedType: chatRoomTypes) => {
    const accessToken = localStorage.getItem('accessToken');
    try {
      // Call API to update the chatroom
      // await updateChatRoom(chatRoomId, updatedName, updatedType, accessToken);
  
      // Update state to reflect the edited chatroom
      // setChatRooms((prevRooms) =>
      //   prevRooms.map((room) =>
      //     room.segment === chatRoomId
      //       ? { ...room, title: updatedName, type: updatedType }
      //       : room
      //   )
      // );
    } catch (error) {
      console.error('Error editing chatroom:', error);
    }
  };
  
  

  return (
    <div>
      <form onSubmit={handleSubmit}>
        <TextField
          label="Chatroom Name"
          variant="outlined"
          fullWidth
          value={name}
          onChange={(e) => setName(e.target.value)}
          margin="normal"
        />
        <FormControl fullWidth margin="normal">
          <InputLabel>Chat Room Type</InputLabel>
          <Select
            value={chatRoomType}
            onChange={(e) => setChatRoomType(e.target.value as chatRoomTypes)}
            label="Chat Room Type"
            fullWidth
          >
            {chatRoomTypesList.map((type) => (
              <MenuItem key={type} value={type}>
                {type.charAt(0).toUpperCase() + type.slice(1)}
              </MenuItem>
            ))}
          </Select>
        </FormControl>
        <Button type="submit" variant="contained" color="primary" fullWidth>
          Create Chatroom
        </Button>
      </form>
      <div>
  <h3>Existing Chatrooms</h3>
  <ul>
  {chatRooms.map((room) => (
    <li
      key={room.segment}
      style={{
        marginBottom: '10px',
        display: 'flex',
        alignItems: 'center',
        border: '1px solid #ccc',
        borderRadius: '5px',
        padding: '10px',
      }}
    >
      {/* Render the icon */}
      <div style={{ marginRight: '10px' }}>{room.icon}</div>

      {/* Render the title */}
      <div style={{ flex: 1 }}>
        <strong>{room.title}</strong>
      </div>

      {/* Edit button */}
      <Button
        onClick={() => handleEdit(room.segment, 'Updated Name', 'casual')} 
        variant="outlined"
        color="primary"
        size="small"
        style={{ marginRight: '10px' }}
      >
        Edit
      </Button>

      {/* Delete button */}
      <Button
        onClick={() => handleDelete(room.segment)}
        variant="outlined"
        color="secondary"
        size="small"
      >
        Delete
      </Button>
    </li>
  ))}
</ul>

</div>

    </div>
  );
};