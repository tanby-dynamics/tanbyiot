import {Alert, FormControl, FormControlLabel, InputLabel, Radio, RadioGroup, Select, TextField, Typography } from "@mui/material";
import {RuleCondition} from "../../../api/types.t.ts";
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
import { OpenInNew } from "@mui/icons-material";

export function TelemetryConditionFields({condition}: { condition: RuleCondition }) {
    const devicesApi = useDevicesApi();
    const [ deviceMatchingType, setDeviceMatchingType ] = useState(condition.deviceMatchingType);
    const [ telemetryTypeMatchingType, setTelemetryTypeMatchingType ] = useState(condition.telemetryTypeMatchingType);
    const [ telemetryValueMatchingType, setTelemetryValueMatchingType ] = useState(condition.telemetryValueMatchingType);
    const [ telemetryValueMatchingComparisonOperationType, setTelemetryValueMatchingComparisonOperationType ] = useState(condition.telemetryValueMatchingComparisonOperationType);

    const showTelemetryValueMatchingValue =
        telemetryValueMatchingComparisonOperationType == ComparisonOperationType.LessThan
        || telemetryValueMatchingComparisonOperationType == ComparisonOperationType.Equals
        || telemetryValueMatchingComparisonOperationType == ComparisonOperationType.GreaterThan
        || telemetryValueMatchingComparisonOperationType == ComparisonOperationType.NotEquals;
    
    const {
        isPending: isLoadingDevices,
        data: devices
    } = useQuery({
        queryKey: [QueryKeys.Devices],
        queryFn: devicesApi.getAllDevices
    });
    
    return (
        <>
            <Typography variant={"subtitle1"} sx={{ paddingTop: 1 }}>Device</Typography>
            <FormControl sx={{ paddingTop: 0 }}>
                <RadioGroup row
                            name={"deviceMatchingType"}
                            defaultValue={condition.deviceMatchingType}
                            onChange={(e) => setDeviceMatchingType(e.target.value as DeviceMatchingType)}>
                    <FormControlLabel value={DeviceMatchingType.AllDevices}
                                      control={<Radio/>}
                                      label={"All devices"}/>
                    <FormControlLabel value={DeviceMatchingType.SingleDevice}
                                      control={<Radio/>}
                                      label={"Single device"}/>
                    <FormControlLabel value={DeviceMatchingType.DeviceGroups}
                                      control={<Radio/>}
                                      label={"Device groups"}/>
                </RadioGroup>
            </FormControl>
            {deviceMatchingType === DeviceMatchingType.SingleDevice && <>
                <FormControl fullWidth>
                    {isLoadingDevices && <span>Loading devices...</span>}
                    {devices && devices.length === 0 && (
                        <Alert severity={"warning"}>No devices have been configured. Add a device first.</Alert>
                    )}
                    {devices && devices.length > 0 && (<>
                        <InputLabel id={"device-select-label"}>Device</InputLabel>
                        <Select name={"deviceMatchingId"}
                                label={"Device"}
                                defaultValue={condition.deviceMatchingId ?? ""}
                                labelId={"device-select-label"}
                                required
                                fullWidth>
                            {devices.map(x => <option value={x.id} key={x.id}>{x.name} ({x.id})</option>)}
                        </Select>
                    </>)}
                </FormControl>
            </>}
            {deviceMatchingType === DeviceMatchingType.DeviceGroups && <>
                <FormControl>
                    <TextField margin={"dense"}
                               type={"text"}
                               name={"deviceMatchingGroups"}
                               fullWidth
                               helperText={<>Separate multiple groups with a comma (e.g. <code>room-1, room-2</code>)</>}
                               defaultValue={condition.deviceMatchingGroups ?? ""}/>
                </FormControl>
            </>}
            
            <Typography variant={"subtitle1"} sx={{ paddingTop: 2 }}>Telemetry</Typography>
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
            {telemetryTypeMatchingType === TelemetryTypeMatchingType.SpecifiedTypes && (
                <FormControl>
                    <TextField margin={"dense"}
                               type={"text"}
                               name={"telemetryTypeMatchingSpecifiedTypes"}
                               fullWidth
                               helperText={<>Separate multiple types with a comma (e.g. <code>temp-1, temp-3</code>).</>}
                               defaultValue={condition.telemetryTypeMatchingSpecifiedTypes ?? ""}/>
                </FormControl>
            )}

            {/* Value matching */}
            <Typography variant={"subtitle1"} sx={{ paddingTop: 2 }}>Value</Typography>
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
            {telemetryValueMatchingType === TelemetryValueMatchingType.ParsePayload && <>
                <FormControl>
                    <TextField margin={"dense"}
                               label={"JSONPath"}
                               type={"text"}
                               name={"telemetryValueMatchingPayloadPath"}
                               fullWidth
                               helperText={<>See <a href={"https://jsonpath.com"} target={"_blank"}>jsonpath.com <OpenInNew sx={{ width: 12, height: 12 }}/></a> for an online JSONPath evaluator</>}
                               defaultValue={condition.telemetryValueMatchingPayloadPath}/>
                </FormControl>
            </>}
            {(telemetryValueMatchingType === TelemetryValueMatchingType.UseValue || telemetryValueMatchingType === TelemetryValueMatchingType.ParsePayload) && <>
                <FormControl>
                    <RadioGroup row
                                name={"telemetryValueMatchingComparisonOperationType"}
                                defaultValue={condition.telemetryValueMatchingComparisonOperationType}
                                onChange={(e) => setTelemetryValueMatchingComparisonOperationType(e.target.value as ComparisonOperationType)}>
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
                {showTelemetryValueMatchingValue && <>
                    <TextField margin={"dense"}
                               label={"Comparison value"}
                               type={"text"}
                               name={"telemetryValueMatchingValue"}
                               fullWidth
                               defaultValue={condition.telemetryValueMatchingValue}/>
                </>}
            </>}
        </>
    );
}