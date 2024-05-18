import {Alert, Button, CircularProgress, IconButton, LinearProgress, Tooltip } from "@mui/material";
import { useQuery, useQueryClient } from "@tanstack/react-query";
import {devicesApi, getAllDevices} from "./api/DevicesApi.ts";
import { AddCircleOutlined, Check, ContentCopy, Delete, Edit, HistoryOutlined } from "@mui/icons-material";
import { useState } from "react";
import {AddDeviceDialog} from "./components/devices/AddDeviceDialog.tsx";
import { DataGrid, GridActionsCellItem, GridColDef } from "@mui/x-data-grid";
import {Device} from "./api/DevicesApi.t.ts";
import {formatRelativeTimestamp} from "./helpers/formatting.ts";

function renderIdCell(device: Device) {
    const [didCopy, setDidCopy] = useState(false);
    
    async function copyToClipboard(value: string) {
        try {
            await navigator.clipboard.writeText(value);
            setDidCopy(true);
            setTimeout(() => setDidCopy(false), 2000);
        } catch (error) {
            console.error("Couldn't copy to clipboard", error);
            setDidCopy(false);
            alert("Couldn't copy to clipboard");
        }
    }
    return(
        <>
            <code>{device.id}</code>
            <Tooltip title={didCopy ? "Copied!" : "Copy"}>
                <IconButton onClick={() => copyToClipboard(device.id)}>
                    <ContentCopy/>
                </IconButton>
            </Tooltip>
        </>
    );
}

async function copyTenantId() {
    await navigator.clipboard.writeText("de37f1e6-70a1-4c69-bdbc-317ff86b5267");
}

export function Devices() {
    const queryClient = useQueryClient();
    const [ openAddDeviceDialog, setOpenAddDeviceDialog ] = useState(false);
    const [ isAddingDevice, setIsAddingDevice ] = useState(false);
    const [ newDeviceId, setNewDeviceId ] = useState<string>();

    const devicesTableColumns: GridColDef<Device>[] = [
        {
            field: "id",
            headerName: "ID",
            width: 350,
            renderCell: (params) => renderIdCell(params.row)
        },
        {
            field: "name",
            headerName: "Name",
            flex: 1
        },
        {
            field: "groupName",
            headerName: "Group",
            flex: 1
        },
        {
            field: "lastConnected",
            headerName: "Last connected",
            width: 150,
            valueGetter: (_, device) => formatRelativeTimestamp(device.lastConnected, "Never")
        },
        {
            field: "actions",
            headerName: "Actions",
            type: "actions",
            getActions: (rowParams) => [
                <GridActionsCellItem icon={<HistoryOutlined/>}
                                     label={"View device"}
                                     onClick={() => alert(`View device ${rowParams.row.id}`)}/>,
                <GridActionsCellItem icon={<Edit/>}
                                     label={"Edit"}
                                     onClick={() => alert(`Edit ${rowParams.row.id}`)}/>,
                <GridActionsCellItem icon={<Delete/>}
                                     label={"Delete"}
                                     onClick={() => alert(`delete ${rowParams.row.id}`)}/>
            ]
        }
    ];
    
    const { 
        isPending, 
        isError, 
        error, 
        data: devices 
    } = useQuery({
        queryKey: ["devices"],
        queryFn: getAllDevices
    })
    
    setInterval(() => queryClient.invalidateQueries({
        queryKey: ["devices"]
    }), 5000);
    
    async function addDevice(name: string, groupName: string) {
        setIsAddingDevice(true);
        
        const response = await devicesApi.addDevice(name, groupName);
        
        setIsAddingDevice(false);
        setNewDeviceId(response.id);
        await queryClient.invalidateQueries({
            queryKey: ["devices"]
        })
    }
    
    if (isError) {
        return <Alert severity={"error"}>Error getting devices: {error.name}, {error.message}</Alert>
    }
    
    if (isPending) {
        return <LinearProgress/>;
    }
    
    return (
        <>
            <h3>Devices</h3>
            
            <Button variant={"contained"}
                    onClick={() => copyTenantId()}>
                Copy tenant ID
            </Button>
            
            {devices && devices.length === 0 && (
                <Alert severity={"warning"}>You have no devices. You should create one now!</Alert>
            )}
            
            <Button variant={"contained"} 
                    startIcon={<AddCircleOutlined/>}
                    onClick={() => setOpenAddDeviceDialog(true)}>
                New device
            </Button>
            <AddDeviceDialog open={openAddDeviceDialog}
                             onClose={() => setOpenAddDeviceDialog(false)}
                             onSubmit={addDevice}/>
            {isAddingDevice && <CircularProgress/>}
            {newDeviceId && (
                <Alert icon={<Check/>}
                       severity={"success"}>
                    Your new device has been added.<br/>
                    Next step is to set up your device using this device ID: <code>{newDeviceId}</code>
                </Alert>
            )}
            
            <DataGrid columns={devicesTableColumns}
                      rows={devices}
                      style={{
                          height: 500,
                          width: '100%'
                      }}/>            
        </>
    );
}