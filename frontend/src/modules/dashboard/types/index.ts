import { z } from "zod";
import { filerSchema } from "../schemas/index";

export type fileSchemaType = z.infer<typeof filerSchema>;