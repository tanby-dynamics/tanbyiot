import {FormControl, FormControlLabel, Radio, RadioGroup, TextField, Typography } from "@mui/material";
import {RuleCondition} from "../../../api/types.t.ts";
import {FormRow} from "../../shared/FormRow.tsx";
import {RuleConditionTelemetryTypeType} from "../../../api/enums.ts";
import { useState } from "react";

export function TelemetryConditionFields({condition}: { condition: RuleCondition }) {
    const [ telemetryTypeType, setTelemetryTypeType ] = useState(condition.telemetryTypeType);
    console.log(condition)
    return (
        <>
            {/* Telemetry types */}
            <Typography variant={"h6"}>Telemetry types</Typography>
            <FormRow>
                <FormControl>
                    <RadioGroup row
                                name={"telemetryTypeType"}
                                defaultValue={condition.telemetryTypeType}
                                onChange={(e) => setTelemetryTypeType(e.target.value as RuleConditionTelemetryTypeType)}>
                        <FormControlLabel value={RuleConditionTelemetryTypeType.AllTypes}
                                          control={<Radio/>}
                                          label={"All types"}/>
                        <FormControlLabel value={RuleConditionTelemetryTypeType.SpecifiedTypes}
                                          control={<Radio/>}
                                          label={"Specified types"}/>
                    </RadioGroup>
                </FormControl>
            </FormRow>
            {telemetryTypeType === RuleConditionTelemetryTypeType.SpecifiedTypes && (
                <FormRow>
                    <FormControl>
                        <TextField margin={"dense"}
                                   label={"Telemetry type"}
                                   type={"text"}
                                   name={"comparisonValue"}
                                   fullWidth
                                   helperText={<>Enter telemetry types to match against. Separate types with a comma
                                       (e.g. <code>temp, temperature</code>).</>}
                                   defaultValue={condition.comparisonValue ?? ""}/>
                    </FormControl>
                </FormRow>                
            )}

            {/* Device matching */}
            <Typography variant={"h6"}>Device matching</Typography>

            {/* Value matching */}
            <Typography variant={"h6"}>Value matching</Typography>
        </>
    );
}