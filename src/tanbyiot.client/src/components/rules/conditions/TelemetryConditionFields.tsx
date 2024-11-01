import {Alert, FormControl, FormControlLabel, InputLabel, Radio, RadioGroup, Select, TextField, Typography } from "@mui/material";
import {RuleCondition} from "../../../api/types.t.ts";
import {FormRow} from "../../shared/FormRow.tsx";
import {
    ComparisonOperationType,
    DeviceMatchingType,
    TelemetryTypeMatchingType,
    TelemetryValueMatchingType
} from "../../../api/enums.ts";
import { useState } from "react";
import {useDevicesApi} from "../../../api/DevicesApi.ts";
import {QueryKeys} from "../../../api/constants.ts";
import { useQuery } from "@tanstack/react-query";

export function TelemetryConditionFields({condition}: { condition: RuleCondition }) {
    const devicesApi = useDevicesApi();
    const [ deviceMatchingType, setDeviceMatchingType ] = useState(condition.deviceMatchingType);
    const [ telemetryTypeMatchingType, setTelemetryTypeMatchingType ] = useState(condition.telemetryTypeMatchingType);
    const [ telemetryValueMatchingType, setTelemetryValueMatchingType ] = useState(condition.telemetryValueMatchingType);

    const {
        isPending: isLoadingDevices,
        data: devices
    } = useQuery({
        queryKey: [QueryKeys.Devices],
        queryFn: devicesApi.getAllDevices
    });
    
    return (
        <>
            <Typography variant={"h6"}>Device</Typography>
            <FormRow>
                <FormControl>
                    <RadioGroup row
                                name={"deviceMatchingType"}
                                defaultValue={condition.deviceMatchingType}
                                onChange={(e) => setDeviceMatchingType(e.target.value as DeviceMatchingType)}>
                        <FormControlLabel value={TelemetryTypeMatchingType.AllTypes}
                                          control={<Radio/>}
                                          label={"All types"}/>
                        <FormControlLabel value={TelemetryTypeMatchingType.SpecifiedTypes}
                                          control={<Radio/>}
                                          label={"Specified types"}/>
                    </RadioGroup>
                </FormControl>
            </FormRow>
            {deviceMatchingType === DeviceMatchingType.SingleDevice && <>
                <FormRow>
                    <FormControl fullWidth>
                        {isLoadingDevices && <span>Loading devices...</span>}
                        {devices && devices.length === 0 && (
                            <Alert severity={"warning"}>No devices have been configured. Add a device first.</Alert>
                        )}
                        {devices && devices.length > 0 && (<>
                            <InputLabel id={"device-select-label"}>Device</InputLabel>
                            <Select name={"deviceMatchingId"}
                                    label={"Device"}
                                    defaultValue={condition.deviceMatchingId}
                                    labelId={"device-select-label"}
                                    required
                                    fullWidth>
                                {devices.map(x => <option value={x.id}>{x.name} ({x.id})</option>)}
                            </Select>
                        </>)}
                    </FormControl>
                </FormRow>
            </>}
            {deviceMatchingType === DeviceMatchingType.DeviceGroups && <>
                <FormRow>
                    <FormControl>
                        <TextField margin={"dense"}
                                   type={"text"}
                                   name={"deviceMatchingGroups"}
                                   fullWidth
                                   helperText={<>Separate multiple groups with a comma (e.g. <code>room-1, room-2</code>)</>}
                                   defaultValue={condition.deviceMatchingGroups ?? ""}/>
                    </FormControl>
                </FormRow>
            </>}
            
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
                                   type={"text"}
                                   name={"telemetryTypeMatchingSpecifiedTypes"}
                                   fullWidth
                                   helperText={<>Separate multiple types with a comma (e.g. <code>temp-1, temp-3</code>).</>}
                                   defaultValue={condition.telemetryTypeMatchingSpecifiedTypes ?? ""}/>
                    </FormControl>
                </FormRow>                
            )}

            {/* Value matching */}
            <Typography variant={"h6"}>Value</Typography>
            <FormRow>
                <FormControl>
                    <RadioGroup row
                                name={"telemetryValueMatchingType"}
                                defaultValue={condition.telemetryValueMatchingType}
                                onChange={(e) => setTelemetryValueMatchingType(e.target.value as TelemetryValueMatchingType)}>
                        <FormControlLabel value={TelemetryValueMatchingType.AlwaysMatch}
                                          control={<Radio/>}
                                          label={"Always match"}/>
                        <FormControlLabel value={TelemetryValueMatchingType.UseValue}
                                          control={<Radio/>}
                                          label={"Use value"}/>
                        <FormControlLabel value={TelemetryValueMatchingType.ParsePayload}
                                          control={<Radio/>}
                                          label={"Parse payload"}/>
                    </RadioGroup>
                </FormControl>
            </FormRow>
            {telemetryValueMatchingType === TelemetryValueMatchingType.UseValue && <>
                <FormRow>
                    <TextField margin={"dense"}
                               label={"JSONPath"}
                               type={"text"}
                               name={"telemetryValueMatchingPayloadPath"}
                               fullWidth
                               helperText={<>See <a href={"https://jsonpath.com"} target={"_blank"}>jsonpath.com</a> for an online JSONPath evaluator</>}
                               defaultValue={condition.telemetryValueMatchingPayloadPath}/>
                </FormRow>
            </>}
            {(telemetryValueMatchingType === TelemetryValueMatchingType.UseValue || telemetryValueMatchingType === TelemetryValueMatchingType.ParsePayload) && <>
                <FormRow>
                    <FormControl>
                        <RadioGroup row
                                    name={"telemetryValueMatchingComparisonOperationType"}
                                    defaultValue={condition.telemetryValueMatchingComparisonOperationType}>
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
                <FormRow>
                    <TextField margin={"dense"}
                               label={"Comparison value"}
                               type={"text"}
                               name={"telemetryValueMatchingValue"}
                               fullWidth
                               defaultValue={condition.telemetryValueMatchingValue}/>
                </FormRow>
            </>}
        </>
    );
}