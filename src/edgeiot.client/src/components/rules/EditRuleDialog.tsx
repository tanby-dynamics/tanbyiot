import { useState } from "react";
import {Rule} from "../../api/types.t.ts";
import {useRulesApi} from "../../api/RulesApi.ts";
import { toast } from "react-toastify";
import {Box, Button, Container, Drawer, FormControl, Stack, TextField, Typography } from "@mui/material";
import {FormRow} from "../shared/FormRow.tsx";

export type EditRuleDialogProps = {
    open: boolean;
    rule: Rule | null;
    onClose: () => void;
    onSubmit: () => void;
}

export function EditRuleDialog(props: EditRuleDialogProps) {
    if (props.rule === null) {
        return null;
    }
    
    const [ updating, setUpdating ] = useState(false);
    const rulesApi = useRulesApi();
    
    async function onSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();

        const formData = new FormData(event.currentTarget);
        const formJson = Object.fromEntries((formData as any).entries());

        setUpdating(true);

        try {
            await rulesApi.updateRule(props.rule!.id, {
                name: formJson.name,
                enabled: props.rule!.enabled
            });
            toast.success("Saved rule");
            props.onSubmit();
            props.onClose();
        } catch (error) {
            console.error("Error saving rule", error);
            toast.error("Error saving rule");
        }
        setUpdating(false);
    }
    
    return (
        <>
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
                        <strong>{props.rule.name}</strong> rule
                    </Typography>
                    
                    <Box sx={{ marginTop: 2 }}>
                        <FormRow>
                            <FormControl fullWidth>
                                <TextField name={"name"}
                                           label={"Name"}
                                           required
                                           fullWidth
                                           defaultValue={props.rule.name}/>
                            </FormControl>
                        </FormRow>
                        <FormRow>
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
        </>
    );
}