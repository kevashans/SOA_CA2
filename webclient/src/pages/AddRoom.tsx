import * as React from 'react';
import { TextField, MenuItem, Select, FormControl, InputLabel, Button } from '@mui/material';
import { createChatRoom } from '../api/actionRequests';
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
}: {
  addChatRoom: (name: string, chatRoomType: chatRoomTypes, chatRoomId: string) => void;
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
    </div>
  );
};