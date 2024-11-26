import { z } from "zod";

export const loginrSchema = z.object({
    email: z.string().email({message: "A valid email is required"}),
    password:
    z.string()
    .min(8, {message: "A password must be at least 8 characters long"})
    .max(25, {message: "A password must be less than 25 characters"})
    .regex(/^[a-zA-Z\d_-]{8,}$/, {
      message: "Password can only contain letters, digits, _, and -",
    }),
});