import { useState, useRef, useEffect } from "react";
import { FaUserCircle } from "react-icons/fa";
import { Link, useNavigate } from "react-router-dom"; // Asegúrate de usar react-router-dom
import logo from "@/assets/common/logo-title.svg";
import useAppStore from "@/store/useAppStore";

const Navbar: React.FC = () => {
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const menuRef = useRef<HTMLDivElement | null>(null);
  const logout = useAppStore((state) => state.logout);
  const navigate = useNavigate();

  // Maneja el clic fuera del menú para cerrarlo
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (menuRef.current && !menuRef.current.contains(event.target as Node)) {
        setIsMenuOpen(false);
      }
    };

    document.addEventListener("mousedown", handleClickOutside);
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, []);

  const handleLogout = () => {
    logout(); // Limpia el estado global
    navigate("/login"); // Redirige al login
  };

  return (
    <div className="bg-gray-100 border-b border-gray-300 p-4 flex items-center justify-between">
      {/* Logo o Nombre del Proyecto */}
      <Link to="/">
        <img src={logo} alt="Notepad AI" className="p-4 w-44" />
      </Link>

      {/* Icono de perfil y menú desplegable */}
      <div className="relative" ref={menuRef}>
        <FaUserCircle
          className="text-2xl text-gray-600 cursor-pointer"
          onClick={() => setIsMenuOpen((prev) => !prev)}
        />
        {isMenuOpen && (
          <div className="absolute right-0 mt-2 w-48 bg-white border rounded shadow-md">
            <ul className="text-sm text-gray-700">
              <li
                className="p-2 hover:bg-gray-100 cursor-pointer"
                onClick={() => console.log("Perfil clickeado")}
              >
                Profile
              </li>
              <li
                className="p-2 hover:bg-gray-100 cursor-pointer"
                onClick={() => console.log("Configuraciones clickeado")}
              >
                Settings
              </li>
              <li
                className="p-2 hover:bg-gray-100 cursor-pointer"
                onClick={handleLogout}
              >
                Log out
              </li>
            </ul>
          </div>
        )}
      </div>
    </div>
  );
};

export default Navbar;
