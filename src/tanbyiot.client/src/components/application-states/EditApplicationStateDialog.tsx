import { ApplicationState } from "../../api/types.t";
import {useApplicationStatesApi} from "../../api/ApplicationStatesApi.ts";
import { toast } from "react-toastify";
import {FormRow} from "../shared/FormRow.tsx";
import {Box, Button, Container, Drawer, FormControl, FormLabel, Stack, TextField, Typography } from "@mui/material";
import { useState } from "react";
import { Editor } from "@monaco-editor/react";

export type EditApplicationStateDialogProps = {
    open: boolean;
    state: ApplicationState | null;
    onClose: () => void;
    onSubmit: () => void;
}

export function EditApplicationStateDialog(props: EditApplicationStateDialogProps) {
    if (props.state === null) {
        return null;
    }
    
    const [ value, setValue ] = useState(props.state?.value ?? "");
    const [ updating, setUpdating ] = useState(false);
    const applicationStatesApi = useApplicationStatesApi();
    
    async function onSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        
        if (!value) {
            alert("Value/payload is required");
            return;
        }

        const formData = new FormData(event.currentTarget);
        const formJson = Object.fromEntries((formData as any).entries());
        
        setUpdating(true);
        
        try {
            await applicationStatesApi.updateApplicationState(props.state!.id, {
                key: formJson.key,
                value
            });
            toast.success("Saved application state");
            props.onSubmit();
            props.onClose();
        } catch (error) {
            console.error("Error saving application state", error);
            toast.error("Error saving application state");
        }        
        setUpdating(false);
    }
    
    return (
        <Drawer anchor={"right"}
                sx={{ zIndex: (theme) => theme.zIndex.drawer + 2 }}
                open={props.open}
                onClose={props.onClose}
                PaperProps={{
                    component: "form",
                    onSubmit
                }}>
            <Container sx={{ p: 2, width: 500 }}>
                <Typography variant={"h4"}>
                    <strong>{props.state.key}</strong> application state
                </Typography>

                <Box sx={{ marginTop: 2 }}>
                    <FormRow>
                        <FormControl fullWidth>
                            <TextField name={"key"}
                                       label={"Key"}
                                       required
                                       fullWidth
                                       defaultValue={props.state.key}/>
                        </FormControl>
                    </FormRow>
                    <FormRow>
                        <FormControl fullWidth>
                            <FormLabel>Value/payload</FormLabel>
                            <Editor height={"20vh"}
                                    defaultLanguage={"json"}
                                    defaultValue={props.state.value ?? ""}
                                    onChange={(x) => setValue(x ?? "")}/>
                        </FormControl>
                    </FormRow>
                </Box>

                <Stack spacing={2} direction={"row"} sx={{ paddingTop: 2, float: "right"}}>
                    <Button variant={"text"} onClick={props.onClose}>
                        Cancel
                    </Button>
                    <Button variant={"contained"} type={"submit"} disabled={updating}>
                        Save changes
                    </Button>
                </Stack>
            </Container>
        </Drawer>
    );
}