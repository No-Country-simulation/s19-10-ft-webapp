import { AxiosBase } from "@/common/axios";
import axios from "axios";

export const uploadFile = async (file: File): Promise<void | { error: string; status?: number }> => {
  const formData = new FormData();
  formData.append("file", file);

  try {
    const response = await AxiosBase.post(`/api/file`, formData);
    // console.log(response)
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

export const sendPrompt = async (prompt: string, chatId = 1, fileId = "2") => {
  try {
    const response = await AxiosBase.post(`/api/chat/askfromfile`,
      { chatId, fileId, prompt },
      { headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJuYW1lIjoiVG9tYXMiLCJsYXN0TmFtZSI6IkNvbGF6byIsImVtYWlsIjoidG9tYXNjb2xhekBnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJVc2VyIiwiZXhwIjoxNzM0MDg4OTc4LCJpc3MiOiJsb2NhbGhvc3QiLCJhdWQiOiJsb2NhbGhvc3QifQ.H78prpmUOYkgzlP5MjkrbbXGcQ4GJK5XOdhJgEeW29o`,
      }
    }
  );
  console.log(response)

    return { text: response.data }; // Respuesta del backend
  } catch (error) {
    console.error("Error al enviar el prompt:", error);
    return "Hubo un error al procesar tu mensaje.";
  }
};

