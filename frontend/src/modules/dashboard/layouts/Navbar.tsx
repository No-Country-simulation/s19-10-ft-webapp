import { useState, useRef, useEffect } from "react";
import { FaUserCircle } from "react-icons/fa";
import logo from '@/assets/common/logo-title.svg'
import { Link } from "react-router";

const Navbar: React.FC = () => {
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const menuRef = useRef<HTMLDivElement | null>(null);

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

  return (
    <div className="bg-gray-100 border-b border-gray-300 p-4 flex items-center justify-between">
      {/* Nombre del proyecto */}
      <Link to="/">
          <img src={logo} alt="notepad ai" className="p-4  w-44" />
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
                onClick={() => console.log("Cerrar sesión clickeado")}
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