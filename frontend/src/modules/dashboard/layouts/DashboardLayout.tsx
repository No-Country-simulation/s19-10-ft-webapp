import Sidebar from "./Sidebar";
import Navbar from "./Navbar";
import ChatWindow from "../chat/ChatWindow";

const DashboardLayout = () => (
  <div className="flex h-screen">
    {/* Sidebar ocupa altura completa */}
    <Sidebar />
    {/* Contenedor principal */}
    <div className="flex flex-1 flex-col">
      {/* Navbar fijo en la parte superior */}
      <Navbar />
      {/* Contenedor del contenido principal */}
      <main className="flex-1 overflow-hidden">
        <ChatWindow />
      </main>
    </div>
  </div>
);

export default DashboardLayout;
