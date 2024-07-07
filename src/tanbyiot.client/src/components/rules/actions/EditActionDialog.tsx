import {
    Box,
    Button,
    Container,
    Drawer,
    Paper,
    Stack,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableRow,
    Typography
} from "@mui/material";
import {Rule, RuleAction} from "../../../api/types.t.ts";
import {formatTimestamp} from "../../../helpers/formatting.ts";
import {formatRuleActionType} from "../../../helpers/helpers.ts";
import {useState} from "react";
import {useRulesApi} from "../../../api/RulesApi.ts";
import {toast} from "react-toastify";
import {RuleActionType} from "../../../api/enums.ts";
import {SendInstructionFields} from "./SendInstructionFields.tsx";
import {SetStateFields} from "./SetStateFields.tsx";

function ActionDetailsTable({ action }: { action: RuleAction }) {
    return (
        <>
            <TableContainer sx={{ marginTop: "1em" }} component={Paper}>
                <Table size={"small"} aria-label={"Action details"}>
                    <TableBody>
                        <TableRow>
                            <TableCell>ID</TableCell>
                            <TableCell>{action.id}</TableCell>
                        </TableRow>
                        <TableRow>
                            <TableCell>Type</TableCell>
                            <TableCell>{formatRuleActionType(action.type)}</TableCell>
                        </TableRow>
                        <TableRow>
                            <TableCell>Created at</TableCell>
                            <TableCell>{formatTimestamp(action.createdAt)}</TableCell>
                        </TableRow>
                        <TableRow>
                            <TableCell>Last updated</TableCell>
                            <TableCell>{formatTimestamp(action.updatedAt)}</TableCell>
                        </TableRow>
                    </TableBody>
                </Table>
            </TableContainer>
        </>
    );
}

export type EditActionDialogProps = {
    open: boolean;
    action: RuleAction | null;
    rule: Rule;
    onClose: () => void;
    onSubmit: () => void;
}

export function EditActionDialog(props: EditActionDialogProps) {
    if (props.action === null) {
        return null;
    }
    
    const [ updating, setUpdating ] = useState(false);
    const rulesApi = useRulesApi();
    const [ payload, setPayload ] = useState(props.action?.payload ?? '');
    
    async function onSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();

        const formData = new FormData(event.currentTarget);
        const formJson = Object.fromEntries((formData as any).entries());
        
        setUpdating(true);
        
        try {
            await rulesApi.updateRuleAction(props.rule.id, props.action!.id, {
                sendInstructionDeviceGroups: formJson.sendInstructionDeviceGroups,
                sendInstructionDeviceId: formJson.sendInstructionDeviceId,
                payload: payload,
                sendInstructionType: formJson.sendInstructionType,
                sendInstructionValue: formJson.sendInstructionValue,
                sendInstructionTargetDeviceType: formJson.sendInstructionTargetDeviceType,
                key: formJson.key
            });
            toast.success("Saved rule action");
            props.onSubmit();
            props.onClose();
        } catch (error) {
            console.error("Error saving rule action", error);
            toast.error("Error saving rule action");
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
                        <strong>{formatRuleActionType(props.action.type)}</strong> action
                    </Typography>       
                    
                    <ActionDetailsTable action={props.action}/>

                    <Box sx={{ marginTop: 2 }}>
                        {props.action.type === RuleActionType.SendInstruction && (
                            <SendInstructionFields action={props.action}
                                                   onPayloadChange={(value) => setPayload(value)}/>
                        )}
                        {props.action.type === RuleActionType.SetState && (
                            <SetStateFields action={props.action}
                                            onPayloadChange={(value) => setPayload(value)}/>
                        )}
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