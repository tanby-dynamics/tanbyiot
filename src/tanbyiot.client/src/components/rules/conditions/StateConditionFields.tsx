import { FormControl, TextField } from "@mui/material";
import {FormRow} from "../../shared/FormRow.tsx";
import {RuleCondition} from "../../../api/types.t.ts";

export type StateConditionFieldsProps = {
    condition: RuleCondition
}

export function StateConditionFields(props: StateConditionFieldsProps) {
    return (
        <>
            <FormRow>
                <FormControl fullWidth>
                    <TextField name={"key"}
                               label={"Key"}
                               defaultValue={props.condition.key}
                               required
                               fullWidth
                               margin={"dense"}/>
                </FormControl>
            </FormRow>
            <FormRow>
                <FormControl fullWidth>
                    <TextField name={"comparisonValue"}
                               label={"Value"}
                               defaultValue={props.condition.comparisonValue}
                               required
                               fullWidth
                               margin={"dense"}/>
                </FormControl>
            </FormRow>
        </>
    );
}