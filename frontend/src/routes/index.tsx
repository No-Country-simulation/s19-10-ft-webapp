import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import Login from '../modules/auth/login/page';
import SignUp from '../modules/auth/signup/page';
import App from '../App';

const router = createBrowserRouter([
  {
    path: '/',
    element: <App />,
  },
  {
    path: '/login',
    element: <Login />
  },
  {
    path: '/signup',
    element: <SignUp />
  }
]);

export default function Router() {
  return <RouterProvider router={router} />;
}