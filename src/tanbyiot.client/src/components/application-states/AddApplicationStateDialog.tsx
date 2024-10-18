import {Button, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, TextField } from "@mui/material";
import {AddApplicationStateArgs} from "../../api/types.t.ts";

interface AddApplicationStateDialogProps {
    open: boolean;
    onClose: () => void,
    onSubmit: (args: AddApplicationStateArgs) => void
}

export function AddApplicationStateDialog(props: AddApplicationStateDialogProps) {
    function onSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        const formData = new FormData(event.currentTarget);
        const args = Object.fromEntries((formData as any).entries()) as AddApplicationStateArgs;
        
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
            <DialogTitle>Add application state</DialogTitle>
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