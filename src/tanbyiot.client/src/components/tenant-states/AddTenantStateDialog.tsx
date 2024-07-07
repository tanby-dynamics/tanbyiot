import {Button, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, TextField } from "@mui/material";
import {AddTenantStateArgs} from "../../api/types.t.ts";

interface AddTenantStateDialogProps {
    open: boolean;
    onClose: () => void,
    onSubmit: (args: AddTenantStateArgs) => void
}

export function AddTenantStateDialog(props: AddTenantStateDialogProps) {
    function onSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        const formData = new FormData(event.currentTarget);
        const args = Object.fromEntries((formData as any).entries()) as AddTenantStateArgs;
        
        props.onSubmit(args);
        props.onClose();
    }
    
    return (
        <Dialog open={props.open}
                onClose={props.onClose}
                PaperProps={{
                    component: 'form',
                    onSubmit
                }}>
            <DialogTitle>Add tenant state</DialogTitle>
            <DialogContent>
                <DialogContentText>Give the state a unique key and value</DialogContentText>
                <TextField autoFocus
                           required
                           margin={"dense"}
                           name={"key"}
                           label={"Key"}
                           type={"text"}
                           fullWidth
                           variant={"standard"}/>
                <TextField margin={"dense"}
                           name={"value"}
                           label={"Value"}
                           type={"text"}
                           fullWidth
                           variant={"standard"}/>
            </DialogContent>
            <DialogActions>
                <Button onClick={props.onClose}>Cancel</Button>
                <Button type={"submit"}>Add</Button>
            </DialogActions>
        </Dialog>
    );
}