import { z } from "zod";
import { signuprSchema } from "../schemas/index";

export type signupSchemaType = z.infer<typeof signuprSchema>;