import axios from "axios";

const BASE_URL = import.meta.env.VITE_APP_API_URL as string;

export const AxiosBase = axios.create({
  baseURL: BASE_URL
});
