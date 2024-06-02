import {
    Box,
    Button,
    Container,
    Drawer,
    FormControl,
    Paper,
    Stack,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableRow,
    TextField,
    Typography
} from "@mui/material";
import {Rule, RuleCondition, RuleConditionType} from "../../api/types.t.ts";
import {formatRuleConditionType} from "../../helpers/helpers.ts";
import {formatTimestamp} from "../../helpers/formatting.ts";
import { useState } from "react";
import {rulesApi} from "../../api/RulesApi.ts";
import { toast } from "react-toastify";
import {FormRow} from "../shared/FormRow.tsx";

function ConditionDetailsTable({ condition }: { condition: RuleCondition }) {
    return (
        <>
            <TableContainer sx={{ marginTop: "1em" }} component={Paper}>
                <Table size={"small"} aria-label={"Condition details"}>
                    <TableBody>
                        <TableRow>
                            <TableCell>ID</TableCell>
                            <TableCell>{condition.id}</TableCell>
                        </TableRow>
                        <TableRow>
                            <TableCell>Type</TableCell>
                            <TableCell>{formatRuleConditionType(condition.type)}</TableCell>
                        </TableRow>
                        <TableRow>
                            <TableCell>Created at</TableCell>
                            <TableCell>{formatTimestamp(condition.createdAt)}</TableCell>
                        </TableRow>
                        <TableRow>
                            <TableCell>Last updated</TableCell>
                            <TableCell>{formatTimestamp(condition.updatedAt)}</TableCell>
                        </TableRow>
                    </TableBody>
                </Table>
            </TableContainer>
        </>
    );
}

function TelemetryTypesFields({ condition }: { condition: RuleCondition }) {
    return (
        <>
            <FormRow>
                <FormControl>
                    <TextField margin={"dense"}
                               label={"Telemetry type"}
                               type={"text"}
                               name={"telemetryTypes"}
                               fullWidth
                               helperText={<>Enter telemetry types to match against. Separate types with a comma (e.g. <code>temp, temperature</code>).</>}
                               defaultValue={condition.comparisonValue ?? ""}/>
                </FormControl>
            </FormRow>            
        </>
    );
}

export type EditConditionDialogProps = {
    open: boolean;
    condition: RuleCondition | null;
    rule: Rule;
    onClose: () => void;
    onSubmit: () => void;
}

export function EditConditionDialog(props: EditConditionDialogProps) {
    if (props.condition === null) {
        return null;
    }
    
    const [ updating, setUpdating ] = useState(false);
    
    async function onSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        
        const formData = new FormData(event.currentTarget);
        const formJson = Object.fromEntries((formData as any).entries());
        
        setUpdating(true);
        
        try {
            await rulesApi.updateRuleCondition(props.rule.id, props.condition!.id, {
                comparisonValue: (() => {
                    switch (props.condition!.type) {
                        case RuleConditionType.deviceId: return "not implement";
                        case RuleConditionType.group: return "not implemented";
                        case RuleConditionType.telemetryTypes: return formJson.telemetryTypes;
                        case RuleConditionType.payload: return "not implemented";
                        case RuleConditionType.value: return "not implemented";
                        default: throw `Can't get comparison value for condition type ${props.condition?.type}`
                    }
                })(),
                comparisonOperation: null,
                payloadPath: null,
                conversionType: null
            });
            toast.success("Saved rule condition");
            props.onSubmit();
            props.onClose();
        } catch (error) {
            console.error("Error saving rule condition", error);
            toast.error("Error saving rule condition")
        }
        setUpdating(false);
    }
    
    return (
        <>
            <Drawer anchor={"right"}
                    sx={{ zIndex: 1202 }}
                    open={props.open}
                    onClose={props.onClose}
                    PaperProps={{
                        component: "form",
                        onSubmit
                    }}>
                <Container sx={{ p: 2, width: 500 }}>
                    <Typography variant={"h4"}>
                        <strong>{formatRuleConditionType(props.condition.type)}</strong> condition
                    </Typography>
                    
                    <ConditionDetailsTable condition={props.condition}/>

                    <Box sx={{ marginTop: 2 }}>
                        {props.condition.type === RuleConditionType.telemetryTypes && <TelemetryTypesFields condition={props.condition}/>}
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