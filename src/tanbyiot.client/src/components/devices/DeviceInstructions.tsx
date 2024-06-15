import {DataGrid, GridColDef } from "@mui/x-data-grid";
import {Device, Instruction} from "../../api/types.t.ts";
import {Alert, Button, LinearProgress, Typography } from "@mui/material";
import { useQuery, useQueryClient } from "@tanstack/react-query";
import {useDevicesApi} from "../../api/DevicesApi.ts";
import {formatTimestamp} from "../../helpers/formatting.ts";
import {CopyValueButton} from "../shared/CopyValueButton.tsx";
import {PayloadCell} from "../shared/PayloadCell.tsx";
import {useEffect } from "react";
import { Helmet } from "react-helmet";

export type DeviceInstructionsProps = {
    device: Device
}

export function DeviceInstructions(props: DeviceInstructionsProps) {
    const queryClient = useQueryClient();
    const devicesApi = useDevicesApi();
    
    const {
        isPending,
        isError,
        error,
        data: instructions
    } = useQuery({
        queryKey: ["device-instructions"],
        queryFn: () =>  devicesApi.getDeviceInstructions(props.device.id)
    });
    
    async function refresh() {
        await queryClient.invalidateQueries({
            queryKey: ["device-instructions"]
        });    
    }

    // Refresh every 10 seconds
    useEffect(() => {
        const refreshTimer = setInterval(refresh,10000);

        return () => clearInterval(refreshTimer);
    }, []);

    const columns: GridColDef<Instruction>[] = [
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
            field: "createdAt",
            headerName: "Created at",
            width: 200,
            valueGetter: (_, instruction) => formatTimestamp(instruction.createdAt)
        },
        {
            field: "sentAt",
            headerName: "Sent at",
            width: 200,
            valueGetter: (_, instruction) => formatTimestamp(instruction.sentAt, "Not sent")
        }
    ];
    
    if (isError) {
        return <Alert severity={"error"}>Error getting device instructions: {error.name}, {error.message}</Alert>;
    }
    
    if (isPending) {
        return <LinearProgress/>;
    }

    return (
        <>
            <Helmet>
                <title>Instructions - {props.device.name} - edgeiot</title>
            </Helmet>
            <Typography align={"right"}>
                <Button onClick={refresh}>Refresh</Button>
            </Typography>
            <Typography align={"right"} variant={"subtitle2"}>
                Automatically refreshes every 10 seconds
            </Typography>

            {instructions && instructions.length === 0 && (
                <Alert severity={"warning"}>There are no instructions recorded for this device.</Alert>
            )}

            {instructions && instructions.length > 0 && (
                <DataGrid columns={columns}
                          rows={instructions}
                          autoHeight
                          style={{
                              width: "100%",
                              marginTop: "1em"
                          }}/>
            )}            
        </>
    );
}