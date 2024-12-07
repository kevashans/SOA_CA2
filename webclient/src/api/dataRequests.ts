export const getChatRooms = async (accessToken) => {
    const response = await fetch(`${process.env.REACT_APP_API_BASE_URL}/api/ChatRooms`, {
        method: 'Get',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${accessToken}`, // Add the Authorization header with the Bearer token
          },
    });
    
    return response
};

export const getChatRoomMessages = async (accessToken: string, chatRoomId: string) => {
    const response = await fetch(`${process.env.REACT_APP_API_BASE_URL}/api/ChatRooms/${chatRoomId}/messages`, {
        method: 'Get',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${accessToken}`, // Add the Authorization header with the Bearer token
          },
    });
    
    return response
}