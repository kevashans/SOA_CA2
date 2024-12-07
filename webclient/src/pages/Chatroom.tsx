import React, { useState, useEffect } from 'react';
import { Box, TextField, Button, Typography, Paper, Stack } from '@mui/material';
import { createChatRoom, sendUserMessage, startChatRoomSession } from '../api/actionRequests';
import { getChatRooms } from '../api/dataRequests';

interface Message {
  messageId: string;
  chatRoomId: string;
  content: string;
  messageType: 'Input' | 'Output'; // 'Input' for user messages, 'Output' for bot responses
  createdAt: string; // Timestamp for when the message was created
}

const getChatRoomMessages = async (accessToken: string, chatRoomId: string) => {
  const response = await fetch(`${process.env.REACT_APP_API_BASE_URL}/api/ChatRooms/${chatRoomId}/messages`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${accessToken}`, // Add the Authorization header with the Bearer token
    },
  });

  return response;
};

const Chatroom = (props: { chatroomId: string; }) => {
    
    const {chatroomId} = props

  const [messages, setMessages] = useState<Message[]>([]);
  const [inputText, setInputText] = useState<string>('');

  // Fetch chat room on component load
  useEffect(() => {
    const accessToken = localStorage.getItem('accessToken'); // Get accessToken from localStorage

    if (!accessToken) {
      console.error('Access token not found');
      return; // Handle the case where the access token is not available
    }

    const fetchChatRoom = async () => {
      try {
        // Fetch chat room data (assuming getChatRoom is a function that fetches the chat room details)

         await startChatRoomSession(chatroomId, accessToken);
        
      } catch (error) {
        console.error('Error fetching chat room:', error);
      }
    };

    fetchChatRoom();
  }, []); // Empty dependency array means this effect runs once on component mount

  // Fetch messages after chat room session is started
  useEffect(() => {
    if (chatroomId) {
      const accessToken = localStorage.getItem('accessToken'); // Get accessToken from localStorage

      if (!accessToken) {
        console.error('Access token not found');
        return;
      }

      const fetchMessages = async () => {
        try {
          const response = await getChatRoomMessages(accessToken, chatroomId);
          if (!response.ok) {
            throw new Error('Failed to fetch chat room messages');
          }

          const data = await response.json();
          setMessages(data.message); // Update messages state with fetched messages
        } catch (error) {
          console.error('Error fetching chat room messages:', error);
        }
      };

      fetchMessages();
    }
  }, [chatroomId]); // Dependency on chatRoomId ensures it runs when the chat room is set

  const handleSend = async () => {
    if (inputText.trim()) {
      const newMessage: Message = {
        messageId: Date.now().toString(), 
        chatRoomId: chatroomId!,
        content: inputText, 
        messageType: 'Input', 
        createdAt: new Date().toISOString(),
      };
      
      setMessages(prevMessages => [...prevMessages, newMessage]); // Add user message to the list
      setInputText(''); // Clear the input field

      const accessToken = localStorage.getItem('accessToken'); // Get accessToken from localStorage

      if (!accessToken) {
        console.error('Access token not found');
        return;
      }

      // Send the user message
      await sendUserMessage(chatroomId, accessToken, inputText);

      // Fetch updated messages after sending the user message
      const fetchUpdatedMessages = async () => {
        try {
          const response = await getChatRoomMessages(accessToken, chatroomId);
          if (!response.ok) {
            throw new Error('Failed to fetch updated messages');
          }

          const data = await response.json();

          // Check if there are messages in the response and then append the last message
          if (data.message && data.message.length > 0) {
            const lastMessage = data.message[data.message.length - 1];
            setMessages(prevMessages => [...prevMessages, lastMessage]); // Append the last message to the previous messages
          }
          
        } catch (error) {
          console.error('Error fetching updated chat room messages:', error);
        }
      };

      fetchUpdatedMessages(); // Fetch updated messages including the bot's response
    }
  };

  return (
    <Box sx={{ maxWidth: 600, margin: 'auto', display: 'flex', flexDirection: 'column', height: '80vh' }}>
      {/* Chat messages display */}
      <Paper sx={{ flex: 1, overflowY: 'auto', padding: 2, maxHeight: '100%', marginBottom: 2 }}>
        <Stack spacing={2}>
          {messages.map((message, index) => (
            <Box key={index} sx={{ display: 'flex', justifyContent: message.messageType === 'Input' ? 'flex-end' : 'flex-start' }}>
              <Typography
                variant="body1"
                sx={{
                  backgroundColor: message.messageType === 'Input' ? 'primary.main' : 'secondary.light',
                  color: message.messageType === 'Input' ? 'white' : 'black',
                  padding: 1,
                  borderRadius: 2,
                  maxWidth: '80%',
                  wordBreak: 'break-word',
                }}
              >
                {message.content}
              </Typography>
              <Typography
                variant="body2"
                sx={{
                  marginLeft: 1,
                  marginTop: 0.5,
                  fontSize: '0.75rem',
                  color: 'gray',
                }}
              >
                {new Date(message.createdAt).toLocaleTimeString()} {/* Display message timestamp */}
              </Typography>
            </Box>
          ))}
        </Stack>
      </Paper>

      {/* Input box and send button */}
      <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
        <TextField
          variant="outlined"
          value={inputText}
          onChange={(e) => setInputText(e.target.value)}
          onKeyDown={(e) => e.key === 'Enter' && handleSend()}
          fullWidth
          size="small"
        />
        <Button variant="contained" onClick={handleSend}>
          Send
        </Button>
      </Box>
    </Box>
  );
};

export default Chatroom;
