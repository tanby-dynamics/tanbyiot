import {Button, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, TextField } from "@mui/material";

export type AddRuleDialogProps = {
    open: boolean;
    onClose: () => void;
    onSubmit: (name: string) => void;
}

export function AddRuleDialog(props: AddRuleDialogProps) {
    function onSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        
        const formData = new FormData(event.currentTarget);
        const formJson = Object.fromEntries((formData as any).entries());
        
        props.onSubmit(formJson.name);
        props.onClose();
    }
    
    return (
        <>
            <Dialog open={props.open}
                    onClose={props.onClose}
                    maxWidth={"sm"}
                    fullWidth
                    PaperProps={{
                        component: "form",
                        onSubmit
                    }}>
                <DialogTitle>Add rule</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        Give the rule a unique name.
                    </DialogContentText>
                    <TextField autoFocus
                               required
                               margin={"dense"}
                               id={"name"}
                               name={"name"}
                               label={"Rule name"}
                               type={"text"}
                               fullWidth/>
                </DialogContent>
                <DialogActions>
                    <Button onClick={props.onClose}>Cancel</Button>
                    <Button type={"submit"}>Add</Button>
                </DialogActions>
            </Dialog>
        </>
    );
}