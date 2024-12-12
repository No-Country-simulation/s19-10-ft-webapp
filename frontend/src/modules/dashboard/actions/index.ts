import { AxiosBase } from "@/common/axios";
import axios from "axios";

export const uploadFile = async (file: File): Promise<void | { error: string; status?: number }> => {
  const formData = new FormData();
  formData.append("file", file);

  try {
    const response = await AxiosBase.post(`/api/file`, formData);
    console.log(response)
    return; // Operación exitosa: no retorna nada (void)
  } catch (error) {
    // Manejar errores y devolver un objeto con detalles del error
    return axios.isAxiosError(error) && error.response
      ? {
          error: error.response.data.message || "File upload failed.",
          status: error.response.status,
        }
      : {
          error: "An unexpected error occurred.",
        };
  }
};

