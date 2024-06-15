import { Box } from "@mui/material";
import { PropsWithChildren } from "react";

// A row to be used as part of a form
export function FormRow({children}: PropsWithChildren) {
    return <Box sx={{ paddingTop: 2 }}>{children}</Box>;
}