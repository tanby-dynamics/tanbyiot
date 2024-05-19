import { ContentCopy } from "@mui/icons-material";
import { IconButton } from "@mui/material";
import { Tooltip } from "@mui/material";
import { useState } from "react";

export type CopyValueButtonProps = {
    value: string;
    tooltip?: string;
}

export function CopyValueButton(props: CopyValueButtonProps) {
    const [didCopy, setDidCopy] = useState(false);
    const tooltip = props.tooltip || "Copy";

    async function copyToClipboard() {
        try {
            await navigator.clipboard.writeText(props.value);
            setDidCopy(true);
            setTimeout(() => setDidCopy(false), 2000);
        } catch (error) {
            console.error("Couldn't copy to clipboard", error);
            setDidCopy(false);
            alert("Couldn't copy to clipboard");
        }
    }

    return (
        <Tooltip title={didCopy ? "Copied!" : tooltip}>
            <IconButton onClick={copyToClipboard}>
                <ContentCopy fontSize={"small"}/>
            </IconButton>
        </Tooltip>
    );
}