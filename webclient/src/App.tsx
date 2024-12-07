import * as React from 'react';
import ChatIcon from '@mui/icons-material/Chat';
import { AppProvider } from '@toolpad/core/react-router-dom';
import { useNavigate , Outlet } from "react-router-dom";
import type { Navigation, Session } from '@toolpad/core';
import { SessionContext } from './SessionContext';
import AddIcon from '@mui/icons-material/Add';

const NAVIGATION: Navigation = [
  {
    kind: 'header',
    title: 'Chatrooms',
  },
  {
    title: 'Add Room',
    icon: <AddIcon />,
  },
  {
    segment: 'chatroom',
    title: 'Chatroom',
    icon: <ChatIcon />,
  },
];

const BRANDING = {
  title: 'Chatbox',
};

export default function App() {
  const [session, setSession] = React.useState<Session | null>(null);
  const navigate = useNavigate();

  const signIn = React.useCallback(() => {
    navigate('/sign-in');
  }, [navigate]);

  const signOut = React.useCallback(() => {
    setSession(null);
    navigate('/sign-in');
  }, [navigate]);

  const sessionContextValue = React.useMemo(
    () => ({ session, setSession }),
    [session, setSession],
  );

  return (
    <SessionContext.Provider value={sessionContextValue}>
      <AppProvider
        navigation={NAVIGATION}
        branding={BRANDING}
        session={session}
        authentication={{ signIn, signOut }}
      >
        <Outlet />
      </AppProvider>
    </SessionContext.Provider>
  );
}
