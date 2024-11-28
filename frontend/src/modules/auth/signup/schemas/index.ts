import { z } from "zod";

export const signuprSchema = z
  .object({
    name: z
      .string()
      .min(3, { message: "Name must be at least 3 characters long" })
      .max(50, { message: "Name must be less than 50 characters" })
      .regex(/^[a-zA-Z\s]+$/, {
        message: "Name can only contain letters and spaces",
      }),
    email: z
      .string()
      .email({ message: "A valid email is required" }),
    password: z
      .string()
      .min(8, { message: "A password must be at least 8 characters long" })
      .max(25, { message: "A password must be less than 25 characters" })
      .regex(/^[a-zA-Z\d_-]{8,}$/, {
        message: "Password can only contain letters, digits, _, and -",
      }),
    repeat_password: z.string(),
  })
  .refine((data) => data.password === data.repeat_password, {
    message: "Passwords must match",
    path: ["repeat_password"],
  });