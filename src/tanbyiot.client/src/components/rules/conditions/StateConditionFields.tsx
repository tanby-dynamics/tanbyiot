import { FormControl, FormControlLabel, Radio, RadioGroup, TextField, Typography } from "@mui/material";
import {FormRow} from "../../shared/FormRow.tsx";
import {RuleCondition} from "../../../api/types.t.ts";
import {ApplicationStateMatchingType, ComparisonOperationType} from "../../../api/enums.ts";
import { useState } from "react";
import { OpenInNew } from "@mui/icons-material";

export type StateConditionFieldsProps = {
    condition: RuleCondition
}

export function StateConditionFields(props: StateConditionFieldsProps) {
    const {condition} = props;
    const [applicationStateMatchingType, setApplicationStateMatchingType] = useState(condition.applicationStateMatchingType);
    const [applicationStateComparisonOperationType, setApplicationStateComparisonOperationType] = useState(condition.applicationStateComparisonOperationType);

    const showApplicationStateMatchingValue = 
        applicationStateComparisonOperationType == ComparisonOperationType.LessThan
        || applicationStateComparisonOperationType == ComparisonOperationType.Equals
        || applicationStateComparisonOperationType == ComparisonOperationType.GreaterThan
        || applicationStateComparisonOperationType == ComparisonOperationType.NotEquals;
    
    return (
        <>
            <FormRow>
                <FormControl fullWidth>
                    <TextField name={"applicationStateMatchingKey"}
                               label={"State key"}
                               defaultValue={props.condition.applicationStateMatchingKey}
                               required
                               fullWidth
                               margin={"dense"}/>
                </FormControl>
            </FormRow>
            
            <Typography variant={"h6"}>Value</Typography>
            <FormRow>
                <FormControl>
                    <RadioGroup row
                                name={"applicationStateMatchingType"}
                                defaultValue={props.condition.applicationStateMatchingType}
                                onChange={(e) => setApplicationStateMatchingType(e.target.value as ApplicationStateMatchingType)}>
                        <FormControlLabel value={ApplicationStateMatchingType.UseValue}
                                          control={<Radio/>}
                                          label={"Use value"}/>
                        <FormControlLabel value={ApplicationStateMatchingType.ParsePayload}
                                          control={<Radio/>}
                                          label={"Parse payload"}/>
                    </RadioGroup>
                </FormControl>
            </FormRow>
            {applicationStateMatchingType === ApplicationStateMatchingType.UseValue && <>
                <FormRow>
                    <TextField margin={"dense"}
                               label={"Value"}
                               type={"text"}
                               name={"applicationStateMatchingValue"}
                               fullWidth
                               defaultValue={condition.applicationStateMatchingValue}/>
                </FormRow>
            </>}
            {applicationStateMatchingType === ApplicationStateMatchingType.ParsePayload && <>
                <FormRow>
                    <TextField margin={"dense"}
                               label={"JSONPath"}
                               type={"text"}
                               name={"applicationStateMatchingPayloadPath"}
                               fullWidth
                               helperText={<>See <a href={"https://jsonpath.com"} target={"_blank"}>jsonpath.com <OpenInNew sx={{ width: 12, height: 12 }}/></a> for an online JSONPath evaluator</>}
                               defaultValue={condition.applicationStateMatchingPayloadPath}/>
                </FormRow>
                <FormRow>
                    <FormControl>
                        <RadioGroup row
                                    name={"applicationStateComparisonOperationType"}
                                    defaultValue={props.condition.applicationStateComparisonOperationType}
                                    onChange={(e) => setApplicationStateComparisonOperationType(e.target.value as ComparisonOperationType)}>
                            <FormControlLabel value={ComparisonOperationType.Equals}
                                              control={<Radio/>}
                                              label={"Equals (=)"}/>
                            <FormControlLabel value={ComparisonOperationType.NotEquals}
                                              control={<Radio/>}
                                              label={"Not equals (!=)"}/>
                            <FormControlLabel value={ComparisonOperationType.GreaterThan}
                                              control={<Radio/>}
                                              label={"Greater than (>)"}/>
                            <FormControlLabel value={ComparisonOperationType.LessThan}
                                              control={<Radio/>}
                                              label={"Less than (<)"}/>
                            <FormControlLabel value={ComparisonOperationType.IsTrue}
                                              control={<Radio/>}
                                              label={"Is true"}/>
                            <FormControlLabel value={ComparisonOperationType.IsFalse}
                                              control={<Radio/>}
                                              label={"Is false"}/>
                        </RadioGroup>
                    </FormControl>
                </FormRow>
                {showApplicationStateMatchingValue && <>
                    <FormRow>
                        <TextField margin={"dense"}
                                   label={"Comparison value"}
                                   type={"text"}
                                   name={"applicationStateMatchingValue"}
                                   fullWidth
                                   defaultValue={condition.applicationStateMatchingValue}/>
                    </FormRow>
                </>}
            </>}
        </>
    );
}