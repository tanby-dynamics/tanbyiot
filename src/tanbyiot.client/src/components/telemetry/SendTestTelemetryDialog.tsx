import { Editor } from "@monaco-editor/react";
import {Alert, Button, Dialog, DialogActions, DialogContent, DialogTitle, FormControl, FormLabel, InputLabel,
    Select, TextField } from "@mui/material";
import { useState } from "react";
import {QueryKeys} from "../../api/constants.ts";
import { useQuery } from "@tanstack/react-query";
import {useDevicesApi} from "../../api/DevicesApi.ts";
import {FormRow} from "../shared/FormRow.tsx";
import {useHwApi} from "../../api/HwApi.ts";
import { toast } from "react-toastify";

export type SendTestTelemetryDialogProps = {
    open: boolean,
    onClose: () => void
}

export function SendTestTelemetryDialog(props: SendTestTelemetryDialogProps) {
    const devicesApi = useDevicesApi();
    const hwApi = useHwApi();
    const [ payload, setPayload ] = useState<string | undefined>("");
    const [ sending, setSending ] = useState(false);

    const {
        isPending: isLoadingDevices,
        data: devices
    } = useQuery({
        queryKey: [QueryKeys.Devices],
        queryFn: devicesApi.getAllDevices
    });

    async function onSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        
        const formData = new FormData(event.currentTarget);
        const formJson = Object.fromEntries((formData as any).entries());
        const telemetry = {
            deviceId: formJson.deviceId,
            type: formJson.telemetryType,
            value: formJson.value,
            payload
        };
        
        if (!telemetry.deviceId) {
            alert("Device is required");
            return;
        }
        if (!telemetry.type) {
            alert("Telemetry type is required");
            return;
        }
        
        setSending(true);
        
        try {
            await hwApi.addTelemetry(telemetry);
            toast.success("Telemetry sent")
        } catch (error) {
            console.error("Error sending test telemetry", error);
            toast.error("Error sendng test telementry");
        }
        
        setSending(false);
    }
    
    return (
        <Dialog open={props.open}
                PaperProps={{
                    component: "form",
                    onSubmit
                }}
                fullWidth>
            <DialogTitle>Send test telemetry</DialogTitle>
            <DialogContent>
                <FormRow>
                    <FormControl fullWidth>
                        {isLoadingDevices && <span>Loading devices...</span>}
                        {devices && devices.length === 0 && (
                            <Alert severity={"warning"}>No devices have been configured. Add a device first.</Alert>
                        )}
                        {devices && devices.length > 0 && (<>
                            <InputLabel id={"device-select-label"}>Device</InputLabel>
                            <Select name={"deviceId"}
                                    label={"Device"}
                                    labelId={"device-select-label"}
                                    defaultValue={""}
                                    fullWidth>
                                {devices.map(x => <option value={x.id}>{x.name} ({x.id})</option>)}
                            </Select>
                        </>)}
                    </FormControl>
                </FormRow>
                <FormRow>
                    <TextField margin={"dense"}
                               type={"text"}
                               name={"telemetryType"}
                               label={"Telemetry type"}
                               fullWidth/>
                </FormRow>
                <FormRow>
                    <TextField margin={"dense"}
                               type={"text"}
                               name={"value"}
                               label={"Value"}
                               fullWidth/>
                </FormRow>
                <FormRow>
                    <FormControl fullWidth>
                        <FormLabel>Payload</FormLabel>
                        <Editor height={"20vh"}
                                defaultLanguage={"json"}
                                onChange={(value) => setPayload(value)}/>
                    </FormControl>
                </FormRow>
            </DialogContent>
            <DialogActions>
                <Button onClick={props.onClose}>Close</Button>
                <Button type={"submit"} disabled={sending}>Send</Button>
            </DialogActions>
        </Dialog>
    );
}