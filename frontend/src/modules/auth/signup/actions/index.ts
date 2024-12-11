import axios from "axios";
import { AxiosBase } from "@/common/axios";
import { signupSchemaType } from "../types";

export const signupUser = async (values: signupSchemaType) => {
     try {
    const response = await AxiosBase.post(
      `/api/account/register`,
      values,
    );
    return response.data;
  } catch (error) {
    return axios.isAxiosError(error) && error.response
      ? {
          error: error.response.data.message || "Register failed.",
          status: error.response.status,
        }
      : {
          error: "An unexpected error occurred.",
        };
  }
}
