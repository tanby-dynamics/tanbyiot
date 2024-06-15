import {Alert, Breadcrumbs, Button, CircularProgress, LinearProgress, Tooltip, Typography } from "@mui/material";
import { useQuery, useQueryClient } from "@tanstack/react-query";
import { AddCircleOutlined, Check, Delete, Edit, HistoryOutlined } from "@mui/icons-material";
import { useState } from "react";
import { DataGrid, GridActionsCellItem, GridColDef } from "@mui/x-data-grid";
import { useNavigate } from 'react-router-dom';
import { Helmet } from "react-helmet";
import {useDevicesApi} from "../api/DevicesApi.ts";
import {Device} from "../api/types.t.ts";
import {CopyValueButton} from "../components/shared/CopyValueButton.tsx";
import {formatRelativeTimestamp} from "../helpers/formatting.ts";
import {AddDeviceDialog} from "../components/devices/AddDeviceDialog.tsx";
import {useUser} from "../api/UsersApi.ts";

export function Devices() {
    const queryClient = useQueryClient();
    const navigate = useNavigate();
    const devicesApi = useDevicesApi();
    const [ openAddDeviceDialog, setOpenAddDeviceDialog ] = useState(false);
    const [ isAddingDevice, setIsAddingDevice ] = useState(false);
    const [ newDeviceId, setNewDeviceId ] = useState<string>();
    const [ addDeviceError, setAddDeviceError ] = useState<Error | null>();
    const user = useUser();

    const devicesTableColumns: GridColDef<Device>[] = [
        {
            field: "name",
            headerName: "Name",
            flex: 1,
            renderCell: (params) => <a href={`/devices/${params.row.id}`}>{params.row.name}</a>
        },
        {
            field: "id",
            headerName: "ID",
            width: 350,
            renderCell: (params) => (
                <>
                    <code><a href={`/devices/${params.row.id}`}>{params.row.id}</a></code>
                    <CopyValueButton value={params.row.id} tooltip={"Copy device ID"}/>
                </>
            )
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
            getActions: (params) => [
                <GridActionsCellItem icon={<Tooltip title={"View device details"}><HistoryOutlined/></Tooltip>}
                                     label={"View device details"}
                                     onClick={() => navigate(`/devices/${params.row.id}`)}/>,
                <GridActionsCellItem icon={<Tooltip title={"Edit device"}><Edit/></Tooltip>}
                                     label={"Edit device"}
                                     onClick={() => alert(`Edit ${params.row.id}`)}/>,
                <GridActionsCellItem icon={<Tooltip title={"Delete device"}><Delete/></Tooltip>}
                                     label={"Delete device"}
                                     onClick={() => alert(`delete ${params.row.id}`)}/>
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
        queryFn: devicesApi.getAllDevices,
        refetchInterval: 10000,
        enabled: !!user?.currentTenant
    })
    
    async function refresh() {
        await queryClient.invalidateQueries({
            queryKey: ["devices"]
        })
    }
    
    async function addDevice(name: string, groupName: string) {
        setIsAddingDevice(true);
        setAddDeviceError(null);
        
        try {
            const response = await devicesApi.addDevice(name, groupName);

            setNewDeviceId(response.id);
            await queryClient.invalidateQueries({
                queryKey: ["devices"]
            })
        } catch (error) {
            setAddDeviceError(error as Error);
            console.error("Error adding device", error);
        }

        setIsAddingDevice(false);
    }
    
    if (isError) {
        return <Alert severity={"error"}>Error getting devices: {error.name}, {error.message}</Alert>;
    }
    
    if (isPending) {
        return <LinearProgress/>;
    }
    
    return (
        <>
            <Helmet>
                <title>Devices - tanbyiot.app</title>
            </Helmet>
            <Breadcrumbs aria-label={"Breadcrumbs"}>
                <Typography color={"text.primary"}>Devices</Typography>
            </Breadcrumbs>

            {devices && devices.length === 0 && (
                <Alert severity={"warning"} style={{ marginBottom: "1em" }}>
                    You have no devices. You should create one now!
                </Alert>
            )}
            
            <Typography align={"right"} style={{ paddingBottom: "1em" }}>
                <Button variant={"contained"}
                        startIcon={<AddCircleOutlined/>}
                        onClick={() => setOpenAddDeviceDialog(true)}>
                    New device
                </Button>
            </Typography>
           
            <AddDeviceDialog open={openAddDeviceDialog}
                             onClose={() => setOpenAddDeviceDialog(false)}
                             onSubmit={addDevice}/>
            {isAddingDevice && <CircularProgress/>}
            {addDeviceError && (
                <Alert severity={"error"}
                       style={{ marginBottom: "1em" }}>
                    Error adding device: {addDeviceError.name}, {addDeviceError.message}
                </Alert>
            )}
            {newDeviceId && (
                <Alert icon={<Check/>}
                       severity={"success"}
                       style={{ marginBottom: "1em" }}
                       onClose={() => setNewDeviceId(undefined)}>
                    Your new device has been added.<br/>
                    Configure your device with these details:<br/>
                    <ul>
                        <li>
                            Tenant ID:{" "}
                            <code>{user?.currentTenant?.id}</code> <CopyValueButton value={user?.currentTenant?.id ?? ""}/>
                        </li>
                        <li>
                            Device ID:{" "}
                            <code>{newDeviceId}</code> <CopyValueButton value={newDeviceId}/>
                        </li>
                    </ul>
                </Alert>
            )}

            <Typography align={"right"}>
                <Button onClick={refresh}>Refresh</Button>
            </Typography>
            <Typography align={"right"} variant={"subtitle2"}>
                Automatically refreshes every 10 seconds
            </Typography>

            <DataGrid columns={devicesTableColumns}
                      rows={devices}
                      autoHeight
                      style={{
                          width: '100%',
                          marginTop: "1em"
                      }}/>            
        </>
    );
}