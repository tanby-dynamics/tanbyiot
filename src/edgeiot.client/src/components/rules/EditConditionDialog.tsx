import { Drawer, Typography } from "@mui/material";
import {RuleCondition} from "../../api/types.t.ts";

export type EditConditionDialogProps = {
    open: boolean;
    condition: RuleCondition | null;
    onClose: () => void;
    onSubmit: () => void;
}
export function EditConditionDialog(props: EditConditionDialogProps) {
    return (
        <>
            <Drawer anchor={"right"}
                    style={{ width: 500 }}
                    open={props.open}
                    onClose={props.onClose}>
                <Typography variant={"h1"}>Editing condition</Typography>
                {props.condition?.id || "condition not set"}
            </Drawer>

        </>
    );
}