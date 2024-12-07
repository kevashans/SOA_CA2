export const userRegister = async (email: string, password: string) => {
    
    const response = await fetch(`${process.env.REACT_APP_API_BASE_URL}/register`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ email, password }),
    });
    
    return response
};

export const userLogin = async (email: string, password: string) => {
    const response = await fetch(`${process.env.REACT_APP_API_BASE_URL}/login`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ email, password }),
    });
    
    return response
};

export const createChatRoom = async (name: string, chatRoomType: string, accessToken: string) => {
    const response = await fetch(`${process.env.REACT_APP_API_BASE_URL}/api/ChatRooms`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${accessToken}`, // Add the Authorization header with the Bearer token
      },
      body: JSON.stringify({ name, chatRoomType }),
    });
  
    return response;
  };

export const startChatRoomSession = async (chatRoomId: string,accessToken:string) => {
    const response = await fetch(`${process.env.REACT_APP_API_BASE_URL}/api/chatrooms/${chatRoomId}/sessions/start`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${accessToken}`, // Add the Authorization header with the Bearer token
        },
      });
    
      return response;
};

export const sendUserMessage = async (chatRoomId: string, accessToken: string, message: string) => {
    const response = await fetch(`${process.env.REACT_APP_API_BASE_URL}/api/chatrooms/${chatRoomId}/messages`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${accessToken}`, // Add the Authorization header with the Bearer token
      },
      body: JSON.stringify({ content: message }) // Properly serialize the message into JSON
    });
  
    return response;
  }
  