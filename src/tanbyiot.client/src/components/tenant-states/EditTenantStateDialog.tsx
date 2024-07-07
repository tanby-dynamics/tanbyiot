import { TenantState } from "../../api/types.t";
import {useTenantStatesApi} from "../../api/TenantStatesApi.ts";
import { toast } from "react-toastify";
import {FormRow} from "../shared/FormRow.tsx";
import {Box, Button, Container, Drawer, FormControl, Stack, TextField, Typography } from "@mui/material";
import { useState } from "react";

export type EditTenantStateDialogProps = {
    open: boolean;
    state: TenantState | null;
    onClose: () => void;
    onSubmit: () => void;
}

export function EditTenantStateDialog(props: EditTenantStateDialogProps) {
    if (props.state === null) {
        return null;
    }
    
    const [ updating, setUpdating ] = useState(false);
    const tenantStatesApi = useTenantStatesApi();
    
    async function onSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        
        const formData = new FormData(event.currentTarget);
        const formJson = Object.fromEntries((formData as any).entries());
        
        setUpdating(true);
        
        try {
            await tenantStatesApi.updateTenantState(props.state!.id, {
                key: formJson.key,
                value: formJson.value
            });
            toast.success("Saved tenant state");
            props.onSubmit();
            props.onClose();
        } catch (error) {
            console.error("Error saving tenant state", error);
            toast.error("Error saving tenant state");
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
                    <strong>{props.state.key}</strong> tenant state
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
                            <TextField name={"value"}
                                       label={"Value"}
                                       fullWidth
                                       defaultValue={props.state.value}/>
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