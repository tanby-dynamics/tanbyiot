import {Button, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, FormControl, FormLabel, TextField } from "@mui/material";
import {AddApplicationStateArgs} from "../../api/types.t.ts";
import {FormRow} from "../shared/FormRow.tsx";
import { Editor } from "@monaco-editor/react";
import { useState } from "react";

interface AddApplicationStateDialogProps {
    open: boolean;
    onClose: () => void,
    onSubmit: (args: AddApplicationStateArgs) => void
}

export function AddApplicationStateDialog(props: AddApplicationStateDialogProps) {
    const [ value, setValue ] = useState<string | undefined>("");
    
    function onSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();

        if (!value) {
            alert("Value/payload is required");
            return;
        }

        const formData = new FormData(event.currentTarget);
        const formJson = Object.fromEntries((formData as any).entries());

        if (!formJson.key) {
            alert("Key is required");
            return;
        }
        
        props.onSubmit({
            key: formJson.key,
            value
        });
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
                <DialogContentText>Give the state a unique key and value/payload</DialogContentText>
                <FormRow>
                    <TextField autoFocus
                               required
                               margin={"dense"}
                               name={"key"}
                               label={"Key"}
                               type={"text"}
                               fullWidth
                               variant={"standard"}/>
                </FormRow>
                <FormRow>
                    <FormControl fullWidth>
                        <FormLabel>Value/payload</FormLabel>
                        <Editor height={"20vh"}
                                defaultLanguage={"json"}
                                onChange={(x) => setValue(x)}/>
                    </FormControl>
                </FormRow>
            </DialogContent>
            <DialogActions>
                <Button onClick={props.onClose}>Cancel</Button>
                <Button type={"submit"}>Add</Button>
            </DialogActions>
        </Dialog>
    );
}