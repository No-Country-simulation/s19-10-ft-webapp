import { create } from "zustand";
import { devtools, persist } from "zustand/middleware";

interface AppState {
  user: { email: string; token: string | null } | null;
  isLoggedIn: boolean;
  login: (email: string, token: string) => void;
  logout: () => void;
}

const useAppStore = create<AppState>()(
  devtools(
    persist(
      (set) => ({
        user: null,
        isLoggedIn: false,
        login: (email, token) =>
          set(() => ({ user: { email, token }, isLoggedIn: true })),
        logout: () => set(() => ({ user: null, isLoggedIn: false })),
      }),
      { name: "app-storage" } // Nombre del storage en el navegador
    )
  )
);

export default useAppStore;
