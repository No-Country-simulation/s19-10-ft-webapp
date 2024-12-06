import Sidebar from "./Sidebar";
import Navbar from "./Navbar";
import ChatWindow from "../chat/ChatWindow";

const DashboardLayout = () => (
  <div className="flex">
    <Sidebar />
    <div className="flex-1">
      <Navbar />
      <main className="p-4"><ChatWindow /></main>
    </div>
  </div>
);

export default DashboardLayout;