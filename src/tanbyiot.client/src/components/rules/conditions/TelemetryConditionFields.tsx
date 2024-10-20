import {FormControl, FormControlLabel, Radio, RadioGroup, TextField, Typography } from "@mui/material";
import {RuleCondition} from "../../../api/types.t.ts";
import {FormRow} from "../../shared/FormRow.tsx";
import {TelemetryTypeMatchingType} from "../../../api/enums.ts";
import { useState } from "react";

export function TelemetryConditionFields({condition}: { condition: RuleCondition }) {
    const [ telemetryTypeMatchingType, setTelemetryTypeMatchingType ] = useState(condition.telemetryTypeMatchingType);
    console.log(condition)
    return (
        <>
            {/* Device matching */}
            <Typography variant={"h6"}>Device</Typography>

            {/* Telemetry types */}
            <Typography variant={"h6"}>Telemetry</Typography>
            <FormRow>
                <FormControl>
                    <RadioGroup row
                                name={"telemetryTypeMatchingType"}
                                defaultValue={condition.telemetryTypeMatchingType}
                                onChange={(e) => setTelemetryTypeMatchingType(e.target.value as TelemetryTypeMatchingType)}>
                        <FormControlLabel value={TelemetryTypeMatchingType.AllTypes}
                                          control={<Radio/>}
                                          label={"All types"}/>
                        <FormControlLabel value={TelemetryTypeMatchingType.SpecifiedTypes}
                                          control={<Radio/>}
                                          label={"Specified types"}/>
                    </RadioGroup>
                </FormControl>
            </FormRow>
            {telemetryTypeMatchingType === TelemetryTypeMatchingType.SpecifiedTypes && (
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

            {/* Value matching */}
            <Typography variant={"h6"}>Value</Typography>
        </>
    );
}