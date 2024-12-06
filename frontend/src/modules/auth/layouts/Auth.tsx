import logo from "@/assets/common/logo-title.svg";
import image from "@/assets/auth/login-img.svg";
import { Link, Outlet } from "react-router";

export default function Auth() {
  return (
    <main className="grid md:grid-cols-2 h-screen">
      {/* Left Section */}
      <section className="flex flex-col bg-white justify-between">
        <header className="p-4">
        <Link to="/">
          <img src={logo} alt="notepad ai" className="w-44" />
          </Link>
        </header>
        {/* Outlet Container */}
        <div className="px-12 flex-1 flex flex-col justify-center">
          <Outlet />
        </div>
      </section>

      {/* Right Section */}
      <section className="hidden md:flex bg-black items-center justify-center">
        <div className="p-4 max-w-lg">
          <img
            src={image}
            alt="Illustration"
            className="opacity-80 border-opacity-10"
          />
        </div>
      </section>
    </main>
  );
}
