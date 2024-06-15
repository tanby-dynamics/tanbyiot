import {DataGrid, GridColDef } from "@mui/x-data-grid";
import {useQuery, useQueryClient} from "@tanstack/react-query";
import {Device, Telemetry} from "../../api/types.t.ts";
import {formatTimestamp} from "../../helpers/formatting.ts";
import {CopyValueButton} from "../shared/CopyValueButton.tsx";
import {PayloadCell} from "../shared/PayloadCell.tsx";
import {useDevicesApi} from "../../api/DevicesApi.ts";
import { Alert, Button, LinearProgress, Typography } from "@mui/material";
import { Helmet } from "react-helmet";

export type DeviceTelemetryProps = {
    device: Device
}

export function DeviceTelemetry(props: DeviceTelemetryProps) {
    const queryClient = useQueryClient();
    const devicesApi = useDevicesApi();
    
    const {
        isPending,
        isError,
        error,
        data: telemetries
    } = useQuery({
        queryKey: ["device-telemetries"],
        queryFn: () => devicesApi.getDeviceTelemetries(props.device.id),
        refetchInterval: 10000
    });

    async function refresh() {
        await queryClient.invalidateQueries({
            queryKey: ["device-telemetries"]
        });
    }
    
    const columns: GridColDef<Telemetry>[] = [
        {
            field: "id",
            headerName: "Id",
            renderCell: (params) => (
                <>
                    <CopyValueButton value={params.row.id}/>
                    <code>{params.row.id}</code>
                </>
            ),
            flex: 0.25
        },
        {
            field: "type",
            headerName: "Type",
            flex: 0.25
        },
        {
            field: "value",
            headerName: "Value",
            flex: 0.25
        },
        {
            field: "payload",
            headerName: "Payload",
            flex: 0.5,
            renderCell: (params) => <PayloadCell rawPayload={params.row.payload}/>
        },
        {
            field: "receivedAt",
            headerName: "Received at",
            valueGetter: (_, instruction) => formatTimestamp(instruction.receivedAt),
            width: 200
        }
    ];
    
    
    if (isError) {
        return <Alert severity={"error"}>Error getting device telemetry data: {error.name}, {error.message}</Alert>;
    }

    if (isPending) {
        return <LinearProgress/>;
    }

    return (
        <>
            <Helmet>
                <title>Telemetry - {props.device.name} - edgeiot</title>
            </Helmet>
            
            <Typography align={"right"}>
                <Button onClick={refresh}>Refresh</Button>
            </Typography>
            <Typography align={"right"} variant={"subtitle2"}>
                Automatically refreshes every 10 seconds
            </Typography>

            {telemetries && telemetries.length === 0 && (
                <Alert severity={"warning"}>There is no telemetry data recorded for this device.</Alert>
            )}

            {telemetries && telemetries.length > 0 && (
                <DataGrid columns={columns}
                          rows={telemetries}
                          autoHeight/>
            )}
            
        </>
    );
}