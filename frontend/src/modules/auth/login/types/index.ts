import { z } from "zod";
import { loginrSchema } from "../schemas/index";

export type loginSchemaType = z.infer<typeof loginrSchema>;