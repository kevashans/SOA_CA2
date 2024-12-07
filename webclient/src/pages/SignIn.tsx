'use client';
import { SignInPage } from '@toolpad/core/SignInPage';
import type { Session } from '@toolpad/core/AppProvider';
import { Link, useNavigate } from 'react-router-dom';
import { useSession } from '../SessionContext';
import { userLogin, userRegister } from '../api/actionRequests';

// Store the token in localStorage (or sessionStorage, based on your needs)
const storeToken = (accessToken : string, refreshToken : string) => {
  localStorage.setItem('accessToken', accessToken); // Store the access token
  localStorage.setItem('refreshToken', refreshToken); // Store the refresh token (if needed)
};

const registerUser = async (formData: FormData): Promise<Session> => {
  const email = formData.get('email') as string;
  const password  = formData.get('password') as string;

  // Basic frontend validation
  if (!email || !password) {
    throw new Error('Email and password are required.');
  }

  if (password.length  < 6) {
    throw new Error('Password must be at least 6 characters long.');
  }

  try {
    // Register the user first
    await userRegister(email, password);

    // Now, log in the user
    const response = await userLogin(email, password);
    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(errorData.message || 'Registration or Login failed.');
    }

    const data = await response.json();

    // Store the token
    storeToken(data.accessToken, data.refreshToken);

    // Return the user session data
    return {
      user: {
        email: email,
        id: data.accessToken,
        image: '',
      },
    };
  } catch (error : any) {
    throw new Error(error.message || 'An error occurred during registration or login.');
  }
};


const loginLink = () => {
  return <Link to={"/login"}>Login</Link>
}


export const SignIn = () => {
  const { setSession } = useSession();
  const navigate = useNavigate();

  return (
    <SignInPage
      slots={{
        signUpLink:loginLink
      }}

      providers={[{ id: 'credentials', name: 'Credentials' }]}
      signIn={async (provider, formData, callbackUrl) => {
        try {
          // Call the actual API to register and log in the user
          const session = await registerUser(formData);

          // Store session data in context
          setSession(session);

          // Navigate to the callback URL or default
          navigate(callbackUrl || '/', { replace: true });

          return {};
        } catch (error) {
          return {
            error: error instanceof Error ? error.message : 'An error occurred',
          };
        }
      }}
    />
  );
};
