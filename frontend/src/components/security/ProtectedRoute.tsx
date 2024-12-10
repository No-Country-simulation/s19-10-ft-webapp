import { Navigate } from "react-router-dom";
import useAppStore from "@/store/useAppStore";

const ProtectedRoute = ({ children }: { children: JSX.Element }) => {
  const isLoggedIn = useAppStore((state) => state.isLoggedIn);

  if (!isLoggedIn) {
    return <Navigate to="/login" replace />;
  }

  return children;
};

export default ProtectedRoute;