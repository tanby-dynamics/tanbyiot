import {RuleAction} from "../../../api/types.t.ts";
import {FormRow} from "../../shared/FormRow.tsx";
import {RuleActionSendInstructionTargetDeviceType} from "../../../api/enums.ts";
import { useState } from "react";
import { FormControl, FormControlLabel, FormLabel, Radio, RadioGroup, TextField } from "@mui/material";
import { Editor } from "@monaco-editor/react";

export type SendInstructionFieldsProps = {
    action: RuleAction;
    onPayloadChange: (value: string) => void;
}

export function SendInstructionFields(props: SendInstructionFieldsProps) {
    const [targetDeviceType, setTargetDeviceType] = useState(props.action.sendInstructionTargetDeviceType);

    return (
        <>
            <FormRow>
                <FormControl>
                    <FormLabel id={"target-device-label"}>Target device(s)</FormLabel>
                    <RadioGroup row aria-labelledby={"target-device-label"} name={"sendInstructionTargetDeviceType"}
                                onChange={(e) => setTargetDeviceType(e.target.value as RuleActionSendInstructionTargetDeviceType)}
                                defaultValue={props.action.sendInstructionTargetDeviceType}>
                        <FormControlLabel value={RuleActionSendInstructionTargetDeviceType.SingleDevice}
                                          control={<Radio/>} label={"Single device"}/>
                        <FormControlLabel value={RuleActionSendInstructionTargetDeviceType.DeviceGroups}
                                          control={<Radio/>} label={"Device groups"}/>
                    </RadioGroup>
                </FormControl>
            </FormRow>
            {targetDeviceType === RuleActionSendInstructionTargetDeviceType.SingleDevice && (
                <FormRow>
                    <FormControl fullWidth>
                        <TextField name={"sendInstructionDeviceId"}
                                   label={"Device ID"}
                                   required
                                   fullWidth
                                   defaultValue={props.action.sendInstructionDeviceId}/>
                    </FormControl>
                </FormRow>
            )}
            {targetDeviceType === RuleActionSendInstructionTargetDeviceType.DeviceGroups && (
                <FormRow>
                    <TextField name={"sendInstructionDeviceGroups"}
                               label={"Device groups"}
                               required
                               fullWidth
                               defaultValue={props.action.sendInstructionDeviceGroups}
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
                               defaultValue={props.action.sendInstructionType}
                               helperText={<>Value that will be sent in the <code>type</code> property of the
                                   instruction</>}/>
                </FormControl>
            </FormRow>
            <FormRow>
                <FormControl fullWidth>
                    <TextField name={"sendInstructionValue"}
                               label={"Value to send"}
                               fullWidth
                               defaultValue={props.action.sendInstructionValue}
                               helperText={<>Optional value that will be sent in the <code>value</code> property of the
                                   instruction</>}/>
                </FormControl>
            </FormRow>
            <FormRow>
                <FormControl fullWidth>
                    <FormLabel>Payload</FormLabel>
                    <Editor height={"20vh"}
                            defaultLanguage={"json"}
                            defaultValue={props.action.payload ?? ""}
                            onChange={(value) => props.onPayloadChange(value ?? "")}/>
                </FormControl>
            </FormRow>
        </>
    );
}