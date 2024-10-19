import {RuleAction} from "../../../api/types.t.ts";
import {FormRow} from "../../shared/FormRow.tsx";
import {RuleActionSendInstructionTargetDeviceType} from "../../../api/enums.ts";
import { useState } from "react";
import {
    FormControl, 
    FormControlLabel, 
    FormLabel,
    Radio,
    RadioGroup,
    TextField,
    Select,
    Alert,
    InputLabel
} from "@mui/material";
import { Editor } from "@monaco-editor/react";
import {useDevicesApi} from "../../../api/DevicesApi.ts";
import { useQuery } from "@tanstack/react-query";
import {QueryKeys} from "../../../api/constants.ts";

export type SendInstructionFieldsProps = {
    action: RuleAction;
    onPayloadChange: (value: string) => void;
}

export function SendInstructionFields(props: SendInstructionFieldsProps) {
    const devicesApi = useDevicesApi();
    const [targetDeviceType, setTargetDeviceType] = useState(props.action.sendInstructionTargetDeviceType);

    const {
        isPending: isLoadingDevices,
        data: devices
    } = useQuery({
        queryKey: [QueryKeys.Devices],
        queryFn: devicesApi.getAllDevices
    });
    
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
                        {isLoadingDevices && <span>Loading devices...</span>}
                        {devices && devices.length === 0 && (
                            <Alert severity={"warning"}>No devices have been configured. Add a device first.</Alert>
                        )}
                        {devices && devices.length > 0 && (<>
                            <InputLabel id={"device-select-label"}>Device</InputLabel>
                            <Select name={"sendInstructionDeviceId"}
                                    label={"Device"}
                                    defaultValue={props.action.sendInstructionDeviceId}
                                    labelId={"device-select-label"}
                                    required
                                    fullWidth>
                                {devices.map(x => <option value={x.id}>{x.name} ({x.id})</option>)}
                            </Select>
                        </>)}
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