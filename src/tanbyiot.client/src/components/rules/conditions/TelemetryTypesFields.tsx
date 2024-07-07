import {FormControl, TextField } from "@mui/material";
import {RuleCondition} from "../../../api/types.t.ts";
import {FormRow} from "../../shared/FormRow.tsx";

export function TelemetryTypesFields({condition}: { condition: RuleCondition }) {
    return (
        <>
            <FormRow>
                <FormControl>
                    <TextField margin={"dense"}
                               label={"Telemetry type"}
                               type={"text"}
                               name={"telemetryTypes"}
                               fullWidth
                               helperText={<>Enter telemetry types to match against. Separate types with a comma
                                   (e.g. <code>temp, temperature</code>).</>}
                               defaultValue={condition.comparisonValue ?? ""}/>
                </FormControl>
            </FormRow>
        </>
    );
}