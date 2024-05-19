import {DataGrid, GridColDef } from "@mui/x-data-grid";
import {useQuery, useQueryClient} from "@tanstack/react-query";
import {Telemetry} from "../../api/types.t.ts";
import {formatTimestamp} from "../../helpers/formatting.ts";
import {CopyValueButton} from "../shared/CopyValueButton.tsx";
import {PayloadCell} from "../shared/PayloadCell.tsx";
import {getDeviceTelemetries} from "../../api/DevicesApi.ts";
import { Alert, Button, LinearProgress, Typography } from "@mui/material";
import { useEffect } from "react";

export type DeviceTelemetryProps = {
    deviceId: string
}

export function DeviceTelemetry(props: DeviceTelemetryProps) {
    const queryClient = useQueryClient();
    
    const {
        isPending,
        isError,
        error,
        data: telemetries
    } = useQuery({
        queryKey: ["device-telemetries"],
        queryFn: () => getDeviceTelemetries(props.deviceId)
    });

    async function refresh() {
        await queryClient.invalidateQueries({
            queryKey: ["device-telemetries"]
        });
    }
    
    // Refresh every 10 seconds
    useEffect(() => {
        const refreshTimer = setInterval(refresh, 10000);
        
        return () => clearInterval(refreshTimer);
    }, []);
    
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
            flex: 0.5
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
            flex: 1,
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
            {telemetries && telemetries.length === 0 && (
                <Alert severity={"warning"}>There is no telemetry data recorded for this device.</Alert>
            )}

            <Typography align={"right"}>
                <Button onClick={refresh}>Refresh</Button><br/>
                <Typography variant={"subtitle2"}>Automatically refreshes every 10 seconds</Typography>
            </Typography>

            {telemetries && telemetries.length > 0 && (
                <DataGrid columns={columns}
                          rows={telemetries}
                          autoHeight
                          style={{
                              width: "100%",
                              marginTop: "1em"
                          }}/>
            )}
            
        </>
    );
}