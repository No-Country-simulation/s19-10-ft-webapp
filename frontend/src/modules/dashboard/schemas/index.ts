import { z } from "zod";

export const filerSchema = z.object({
  name: z.string(),
  size: z.number().max(10 * 1024 * 1024, "File size must be less than 10 MB"), // Máximo 10 MB
  type: z.enum(
    [
      "application/pdf",
      "application/msword",
      "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
    ],
    { invalid_type_error: "Invalid file type" }
  ),
});
