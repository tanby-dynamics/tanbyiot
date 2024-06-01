import {
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
    TextField,
    Typography
} from "@mui/material";
import {RuleCondition, RuleConditionType, UpdateRuleConditionArgs} from "../../api/types.t.ts";
import {formatRuleConditionType} from "../../helpers/helpers.ts";
import {formatTimestamp} from "../../helpers/formatting.ts";

export type EditConditionDialogProps = {
    open: boolean;
    condition: RuleCondition | null;
    onClose: () => void;
    onSubmit: (args: UpdateRuleConditionArgs) => void;
}
export function EditConditionDialog(props: EditConditionDialogProps) {
    if (props.condition === null) {
        return null;
    }
    
    function onSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        
        const formData = new FormData(event.currentTarget);
        const formJson = Object.fromEntries((formData as any).entries());

        props.onSubmit({
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
        props.onClose();
    }
    
    return (
        <>
            <Drawer anchor={"right"}
                    open={props.open}
                    onClose={props.onClose}
                    sx={{ zIndex: 1202 }}
                    PaperProps={{
                        component: "form",
                        onSubmit
                    }}>
                <Container sx={{ p: 2, width: 500 }}>
                    <Typography variant={"h4"}>
                        Editing condition
                    </Typography>

                    <TableContainer sx={{ marginTop: "1em" }} component={Paper}>
                        <Table size={"small"} aria-label={"Condition details"}>
                            <TableBody>
                                <TableRow>
                                    <TableCell>ID</TableCell>
                                    <TableCell>{props.condition.id}</TableCell>
                                </TableRow>
                                <TableRow>
                                    <TableCell>Type</TableCell>
                                    <TableCell>{formatRuleConditionType(props.condition.type)}</TableCell>
                                </TableRow>
                                <TableRow>
                                    <TableCell>Last updated</TableCell>
                                    <TableCell>{formatTimestamp(props.condition.updatedAt)}</TableCell>
                                </TableRow>
                            </TableBody>
                        </Table>
                    </TableContainer>

                    {props.condition.type === RuleConditionType.telemetryTypes && (
                        <>
                            <Typography style={{ marginTop: "2em" }}>
                                Enter telemetry types to match against. Separate types with a comma (e.g. <code>temp,temperature</code>).
                            </Typography>
                            <TextField autoFocus
                                       margin={"dense"}
                                       label={"Telemetry type"}
                                       type={"text"}
                                       name={"telemetryTypes"}
                                       fullWidth
                                       variant={"standard"}
                                       defaultValue={props.condition.comparisonValue ?? ""}/>
                        </>
                    )}
                     
                    <Stack spacing={2} direction={"row"} sx={{ paddingTop: 2, float: "right"}}>
                        <Button variant={"text"} onClick={props.onClose}>
                            Cancel
                        </Button>
                        <Button variant={"contained"} type={"submit"}>
                            Save changes
                        </Button>
                    </Stack>
                </Container>
            </Drawer>
        </>
    );
}