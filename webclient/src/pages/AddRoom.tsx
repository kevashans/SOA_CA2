import React, { useState } from 'react';
import { TextField, MenuItem, Select, FormControl, InputLabel, Button, Typography } from '@mui/material';
import { createChatRoom } from '../api/actionRequests';

export const AddChatroomPage = () => {
  const [name, setName] = useState('');
  const [chatRoomType, setChatRoomType] = useState('');

  const chatRoomTypes = ['professional', 'casual', 'pirate'];

  const handleSubmit = (event: React.FormEvent) => {
    event.preventDefault()
    const accessToken = localStorage.getItem("accessToken")
    const res = createChatRoom(name,chatRoomType,accessToken)
    console.log(res)
  };

  return (
    <div>
      <form onSubmit={handleSubmit}>
        {/* Name Field */}
        <TextField
          label="Chatroom Name"
          variant="outlined"
          fullWidth
          value={name}
          onChange={(e) => setName(e.target.value)}
          margin="normal"
        />
        
        {/* Chat Room Type Field */}
        <FormControl fullWidth margin="normal">
          <InputLabel>Chat Room Type</InputLabel>
          <Select
            value={chatRoomType}
            onChange={(e) => setChatRoomType(e.target.value)}
            label="Chat Room Type"
            fullWidth
          >
            {chatRoomTypes.map((type) => (
              <MenuItem key={type} value={type}>
                {type.charAt(0).toUpperCase() + type.slice(1)} {/* Capitalize first letter */}
              </MenuItem>
            ))}
          </Select>
        </FormControl>

        {/* Submit Button */}
        <Button type="submit" variant="contained" color="primary" fullWidth>
          Create Chatroom
        </Button>
      </form>
    </div>
  );
};
