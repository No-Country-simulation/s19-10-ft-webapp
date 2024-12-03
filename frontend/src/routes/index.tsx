import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import Landingpage from '@/modules/home/Landingpage';
import Auth from '@/modules/auth/layouts/Auth';
import LoginForm from '@/modules/auth/login/form';
import SignUpForm from '@/modules/auth/signup/form';
import DashboardLayout from '@/modules/dashboard/layouts/DashboardLayout';

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
  },
  {
    path: '/dashboard',
    element: <DashboardLayout />
  },
]);

export default function Router() {
  return <RouterProvider router={router} />;
}