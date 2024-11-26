import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import Login from '../modules/auth/login/page';
import App from '../App';

const router = createBrowserRouter([
  {
    path: '/',
    element: <App />,
  },
  {
    path: '/login',
    element: <Login />
  }
]);

export default function Router() {
  return <RouterProvider router={router} />;
}