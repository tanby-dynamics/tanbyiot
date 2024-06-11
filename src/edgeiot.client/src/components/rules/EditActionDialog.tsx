import {
    Box,
    Button,
    Container,
    Drawer,
    FormControl,
    FormControlLabel,
    FormLabel,
    Paper,
    Radio,
    RadioGroup,
    Stack,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableRow,
    TextField,
    Typography
} from "@mui/material";
import {Rule, RuleAction, RuleActionSendInstructionTargetDeviceType, RuleActionType} from "../../api/types.t.ts";
import {formatTimestamp} from "../../helpers/formatting.ts";
import {formatRuleActionType} from "../../helpers/helpers.ts";
import { useState } from "react";
import {rulesApi} from "../../api/RulesApi.ts";
import { toast } from "react-toastify";
import {FormRow} from "../shared/FormRow.tsx";

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

function SendInstructionFields({ action }: { action: RuleAction }) {
    const [ targetDeviceType, setTargetDeviceType ] = useState(action.sendInstructionTargetDeviceType);
    
    return (
        <>
            <FormRow>
                <FormControl>
                    <FormLabel id={"target-device-label"}>Target device(s)</FormLabel>
                    <RadioGroup row aria-labelledby={"target-device-label"} name={"sendInstructionTargetDeviceType"}
                                onChange={(e) => setTargetDeviceType(e.target.value as RuleActionSendInstructionTargetDeviceType)}
                                defaultValue={action.sendInstructionTargetDeviceType}>
                        <FormControlLabel value={RuleActionSendInstructionTargetDeviceType.singleDevice} control={<Radio/>} label={"Single device"}/>
                        <FormControlLabel value={RuleActionSendInstructionTargetDeviceType.deviceGroups} control={<Radio/>} label={"Device groups"}/>
                    </RadioGroup>
                </FormControl>  
            </FormRow>            
            {targetDeviceType === RuleActionSendInstructionTargetDeviceType.singleDevice && (
                <FormRow>
                    <FormControl fullWidth>
                        <TextField name={"sendInstructionDeviceId"}
                                   label={"Device ID"}
                                   required
                                   fullWidth
                                   defaultValue={action.sendInstructionDeviceId}/>
                    </FormControl>
                </FormRow>                
            )}
            {targetDeviceType === RuleActionSendInstructionTargetDeviceType.deviceGroups && (
                <FormRow>
                    <TextField name={"sendInstructionDeviceGroups"}
                               label={"Device groups"}
                               required
                               fullWidth
                               defaultValue={action.sendInstructionDeviceGroups}
                               helperText={<>Separate multiple groups with a comma (e.g. <code>room-1, room-2</code></>}
                               />
                </FormRow>
            )}
            <FormRow>
                <FormControl fullWidth>
                    <TextField name={"sendInstructionType"}
                               label={"Instruction type"}
                               required
                               fullWidth
                               defaultValue={action.sendInstructionType}
                               helperText={<>Value that will be sent in the <code>type</code> property of the instruction</>}/>
                </FormControl>
            </FormRow>
            <FormRow>
                <FormControl fullWidth>
                    <TextField name={"sendInstructionValue"}
                               label={"Value to send"}
                               fullWidth
                               defaultValue={action.sendInstructionValue}
                               helperText={<>Optional value that will be sent in the <code>value</code> property of the instruction</>}/>
                </FormControl>
            </FormRow>
            <FormRow>
                <FormControl fullWidth>
                    <TextField name={"sendInstructionPayload"}
                               label={"Payload"}
                               fullWidth
                               defaultValue={action.sendInstructionPayload}
                               helperText={<>Optional JSON (or other text value) that will be sent in the <code>payload</code> property of the instruction</>}
                               multiline/>
                </FormControl>
            </FormRow>
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
    
    async function onSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();

        const formData = new FormData(event.currentTarget);
        const formJson = Object.fromEntries((formData as any).entries());
        
        setUpdating(true);
        
        try {
            await rulesApi.updateRuleAction(props.rule.id, props.action!.id, {
                sendInstructionDeviceGroups: formJson.sendInstructionDeviceGroups,
                sendInstructionDeviceId: formJson.sendInstructionDeviceId,
                sendInstructionPayload: formJson.sendInstructionPayload,
                sendInstructionType: formJson.sendInstructionType,
                sendInstructionValue: formJson.sendInstructionValue,
                sendInstructionTargetDeviceType: formJson.sendInstructionTargetDeviceType
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
                        {props.action.type === RuleActionType.sendInstruction && <SendInstructionFields action={props.action}/>}
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