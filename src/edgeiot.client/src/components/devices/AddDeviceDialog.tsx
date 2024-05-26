import {Button, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, TextField } from "@mui/material";

export type AddDeviceDialogProps = {
    open: boolean;
    onClose: () => void;
    onSubmit: (name: string, groupName: string) => void;
}

export function AddDeviceDialog(props: AddDeviceDialogProps) {
    function onSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        
        const formData = new FormData(event.currentTarget);
        const formJson = Object.fromEntries((formData as any).entries());
        
        props.onSubmit(formJson.name, formJson.groupName);
        props.onClose();
    }
    
    return (
        <Dialog open={props.open}
                onClose={props.onClose}
                PaperProps={{
                    component: 'form',
                    onSubmit
                }}
                >
            <DialogTitle>Add device</DialogTitle>
            <DialogContent>
                <DialogContentText>
                    Give the device a unique name and an optional group name.
                </DialogContentText>
                <TextField autoFocus
                           required
                           margin={"dense"}
                           id={"name"}
                           name={"name"}
                           label={"Device name"}
                           type={"text"}
                           fullWidth
                           variant={"standard"}/>
                <TextField margin={"dense"}
                           id={"groupName"}
                           name={"groupName"}
                           label={"Group name"}
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