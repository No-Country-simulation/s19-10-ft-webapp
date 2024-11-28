import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import Landingpage from '@/modules/Landingpage';
import Auth from '@/modules/auth/templates/Auth';
import LoginForm from '@/modules/auth/login/Form';
import SignUpForm from '@/modules/auth/signup/Form';

const router = createBrowserRouter([
  {
    path: '/',
    element: <Landingpage />,
  },
  {
    element: <Auth />,
    children: [{
      path: '/login',
      element: <LoginForm />
    },
    {
      path: '/signup',
      element: <SignUpForm />
    }
    ]
  }
]);

export default function Router() {
  return <RouterProvider router={router} />;
}