import * as React from 'react';
import * as ReactDOM from 'react-dom/client';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import App from './App';
import { Layout } from './layouts/Dashboard';
import { AddChatroomPage } from './pages/AddRoom';
import Chatroom from './pages/Chatroom';

import { Login } from './pages/Login';
import { SignIn } from './pages/SignIn';

const router = createBrowserRouter([
  {
    Component: App,
    children: [
      {
        path: '/',
        Component: Layout,
        children: [
          {
            path: '/add-room',
            Component: AddChatroomPage,
          },
          {
            path: '/chatroom/:chatRoomId', // Use dynamic parameter here
            Component: Chatroom,
          },
        ],
      },
      {
        path: '/sign-in',
        Component: SignIn,
      },
      {
        path: '/login',
        Component: Login,
      },
    ],
  },
]);

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>,
);
