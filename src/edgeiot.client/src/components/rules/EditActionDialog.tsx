import { Drawer, Typography } from "@mui/material";
import {RuleAction} from "../../api/types.t.ts";

export type EditActionDialogProps = {
    open: boolean;
    action: RuleAction | null;
    onClose: () => void;
    onSubmit: () => void;
}
export function EditActionDialog(props: EditActionDialogProps) {
    return (
        <>
            <Drawer anchor={"right"}
                    style={{ width: 500 }}
                    open={props.open}
                    onClose={props.onClose}>
                <Typography variant={"h1"}>Editing action</Typography>
                {props.action?.id || "action not set"}
            </Drawer>

        </>
    );
}