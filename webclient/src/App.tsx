import * as React from 'react';
import ChatIcon from '@mui/icons-material/Chat';
import { AppProvider } from '@toolpad/core/react-router-dom';
import { useNavigate, Outlet, Routes, Route, Navigate } from "react-router-dom";
import type { Navigation, Session } from '@toolpad/core';
import { SessionContext } from './SessionContext';
import AddIcon from '@mui/icons-material/Add';
import { useState } from 'react';
import { Layout } from './layouts/Dashboard';
import { AddChatroomPage, chatRoomTypes } from './pages/AddRoom';
import Chatroom from './pages/Chatroom';
import Person4Icon from '@mui/icons-material/Person4';
import SailingIcon from '@mui/icons-material/Sailing';
import { Login } from '@mui/icons-material';
import { SignIn } from './pages/SignIn';

const NAVIGATION_BASE: Navigation = [
  { kind: 'header', title: 'Chatrooms' },
  { title: 'Add Room', icon: <AddIcon />, segment: 'add-room' },
];

const BRANDING = {
  title: 'Chatbox',
};

export default function App() {
  const [session, setSession] = React.useState<Session | null>(null);
  const navigate = useNavigate();
  const [chatRooms, setChatRooms] = useState<{ title: string; icon: React.ReactNode; segment: string }[]>([]);

  const signIn = React.useCallback(() => {
    navigate('/sign-in');
  }, [navigate]);

  const signOut = React.useCallback(() => {
    setSession(null);
    navigate('/sign-in');
  }, [navigate]);

  const sessionContextValue = React.useMemo(() => ({ session, setSession }), [session, setSession]);

  // Dynamically calculate navigation
  const navigation = React.useMemo(
    () => [
      ...NAVIGATION_BASE,
      ...chatRooms.map((room) => ({
        segment: `chatroom/${room.segment}`,
        title: room.title,
        icon: room.icon,
      })),
    ],
    [chatRooms]
  );

  const addChatRoom = (name: string, chatRoomType: chatRoomTypes, chatRoomId: string) => {
    let icon = <ChatIcon />;

    switch (chatRoomType) {
      case 'professional':
        icon = <Person4Icon />;
        break;
      case 'pirate':
        icon = <SailingIcon />;
        break;
      default:
        icon = <ChatIcon />;
        break;
    }

    const newChatRoom = { title: name, icon, segment: chatRoomId };
    setChatRooms((prevRooms) => [...prevRooms, newChatRoom]);
  };
  console.log(navigation)

  return (
    <SessionContext.Provider value={sessionContextValue}>
      <AppProvider
        navigation={navigation}
        branding={BRANDING}
        session={session}
        authentication={{ signIn, signOut }}
      >
 <Routes>
  {/* Public Routes */}
  <Route path="/sign-in" element={<SignIn />} />
  <Route path="/login" element={<Login />} />

  {/* Protected Routes */}
  <Route path="/" element={<Layout />}>
    <Route path="add-room" element={<AddChatroomPage addChatRoom={addChatRoom} />} />
    {chatRooms.map((room, index) => (
      <Route
        key={index}
        path={`chatroom/${room.segment}`}
        element={<Chatroom chatroomId={room.segment} />}
      />
    ))}
  </Route>

  {/* Catch-all for undefined routes */}
  <Route path="*" element={<Navigate to="/sign-in" replace />} />
</Routes>
      </AppProvider>
    </SessionContext.Provider>
  );
}
