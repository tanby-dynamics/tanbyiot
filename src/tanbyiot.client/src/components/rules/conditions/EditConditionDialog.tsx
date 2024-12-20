﻿import {
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
import {Rule, RuleCondition} from "../../../api/types.t.ts";
import {formatRuleConditionType} from "../../../helpers/helpers.ts";
import {formatTimestamp} from "../../../helpers/formatting.ts";
import {useState} from "react";
import {useRulesApi} from "../../../api/RulesApi.ts";
import {toast} from "react-toastify";
import {
    RuleConditionType
} from "../../../api/enums.ts";
import {TelemetryConditionFields} from "./TelemetryConditionFields.tsx";
import {StateConditionFields} from "./StateConditionFields.tsx";

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

export type EditConditionDialogProps = {
    open: boolean;
    condition: RuleCondition | null;
    rule: Rule;
    onClose: () => void;
    onSubmit: () => void;
}

export function EditConditionDialog(props: EditConditionDialogProps) {
    const { condition } = props;
    
    if (condition === null) {
        return null;
    }
    
    const [ updating, setUpdating ] = useState(false);
    const rulesApi = useRulesApi();
    
    async function onSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        
        if (condition === null) {
            return;
        }
        
        const formData = new FormData(event.currentTarget);
        const formJson = Object.fromEntries((formData as any).entries());
        
        setUpdating(true);
        
        try {
            await rulesApi.updateRuleCondition(props.rule.id, {
                ruleConditionId: condition.id,
                description: formJson.description,
                applicationStateMatchingKey: formJson.applicationStateMatchingKey,
                applicationStateMatchingType: formJson.applicationStateMatchingType,
                applicationStateMatchingValue: formJson.applicationStateMatchingValue,
                applicationStateMatchingPayloadPath: formJson.applicationStateMatchingPayloadPath,
                applicationStateComparisonOperationType: formJson.applicationStateComparisonOperationType,
                deviceMatchingType: formJson.deviceMatchingType,
                deviceMatchingId: formJson.deviceMatchingId,
                deviceMatchingGroups: formJson.deviceMatchingGroups,
                telemetryTypeMatchingType: formJson.telemetryTypeMatchingType,
                telemetryTypeMatchingSpecifiedTypes: formJson.telemetryTypeMatchingSpecifiedTypes,
                telemetryValueMatchingType: formJson.telemetryValueMatchingType,
                telemetryValueMatchingPayloadPath: formJson.telemetryValueMatchingPayloadPath,
                telemetryValueMatchingComparisonOperationType: formJson.telemetryValueMatchingComparisonOperationType,
                telemetryValueMatchingValue: formJson.telemetryValueMatchingValue
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
                    sx={{ zIndex: (theme) => theme.zIndex.drawer + 2 }}
                    open={props.open}
                    onClose={props.onClose}
                    PaperProps={{
                        component: "form",
                        onSubmit
                    }}>
                <Container sx={{ p: 2, width: 500 }}>
                    <Typography variant={"h4"}>
                        <strong>{formatRuleConditionType(condition.type)}</strong> condition
                    </Typography>
                    
                    <ConditionDetailsTable condition={condition}/>

                    <Box sx={{ marginTop: 2 }}>
                        {condition.type === RuleConditionType.Telemetry && <TelemetryConditionFields condition={condition}/>}
                        {condition.type === RuleConditionType.State && <StateConditionFields condition={condition}/>}
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