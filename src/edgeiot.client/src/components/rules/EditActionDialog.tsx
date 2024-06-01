import {Container, Drawer, Typography } from "@mui/material";
import {RuleAction} from "../../api/types.t.ts";

export type EditActionDialogProps = {
    open: boolean;
    action: RuleAction | null;
    onClose: () => void;
    onSubmit: () => void;
}
export function EditActionDialog(props: EditActionDialogProps) {
    
    function onSubmit() {        
    }
    
    return (
        <>
            <Drawer anchor={"right"}
                    sx={{ zIndex: 1202 }}
                    open={props.open}
                    onClose={props.onClose}
                    PaperProps={{
                        component: "form",
                        onSubmit
                    }}>
                <Container sx={{ p: 2, width: 500 }}>
                    <Typography variant={"h4"}>Editing action</Typography>                    
                </Container>
            </Drawer>

        </>
    );
}