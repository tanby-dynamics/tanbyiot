﻿import {Alert, Button, CircularProgress, LinearProgress, Tooltip } from "@mui/material";
import { useQuery, useQueryClient } from "@tanstack/react-query";
import {devicesApi, getAllDevices} from "./api/DevicesApi.ts";
import { AddCircleOutlined, Check, Delete, Edit, HistoryOutlined } from "@mui/icons-material";
import {useEffect, useState } from "react";
import {AddDeviceDialog} from "./components/devices/AddDeviceDialog.tsx";
import { DataGrid, GridActionsCellItem, GridColDef } from "@mui/x-data-grid";
import {Device} from "./api/types.t.ts";
import {formatRelativeTimestamp} from "./helpers/formatting.ts";
import { useNavigate } from 'react-router-dom';
import {CopyValueButton} from "./components/shared/CopyValueButton.tsx";

export function Devices() {
    const queryClient = useQueryClient();
    const navigate = useNavigate();
    const [ openAddDeviceDialog, setOpenAddDeviceDialog ] = useState(false);
    const [ isAddingDevice, setIsAddingDevice ] = useState(false);
    const [ newDeviceId, setNewDeviceId ] = useState<string>();

    const devicesTableColumns: GridColDef<Device>[] = [
        {
            field: "id",
            headerName: "ID",
            width: 350,
            renderCell: (params) => (
                <>
                    <code><a href={`/device/${params.row.id}`}>{params.row.id}</a></code>
                    <CopyValueButton value={params.row.id} tooltip={"Copy device ID"}/>
                </>
            )
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
                <GridActionsCellItem icon={<Tooltip title={"View device details"}><HistoryOutlined/></Tooltip>}
                                     label={"View device history"}
                                     onClick={() => navigate(`/device/${rowParams.row.id}`)}/>,
                <GridActionsCellItem icon={<Tooltip title={"Edit device"}><Edit/></Tooltip>}
                                     label={"Edit device"}
                                     onClick={() => alert(`Edit ${rowParams.row.id}`)}/>,
                <GridActionsCellItem icon={<Tooltip title={"Delete device"}><Delete/></Tooltip>}
                                     label={"Delete device"}
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

    useEffect(() => {
        const refreshTimer = setInterval(() => queryClient.invalidateQueries({
            queryKey: ["devices"]
        }), 5000);
        
        return () => clearInterval(refreshTimer);
    }, []);    
    
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
        return <Alert severity={"error"}>Error getting devices: {error.name}, {error.message}</Alert>;
    }
    
    if (isPending) {
        return <LinearProgress/>;
    }
    
    return (
        <>
            <h3>Devices</h3>
            
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
                       severity={"success"}
                        style={{ marginTop: "1em" }}>
                    Your new device has been added.<br/>
                    The next step is to set up your device using:<br/>
                    <ul>
                        <li>Tenant ID: <code>de37f1e6-70a1-4c69-bdbc-317ff86b5267</code></li>
                        <li>Device ID: <code>{newDeviceId}</code></li>
                    </ul>
                </Alert>
            )}
            
            <DataGrid columns={devicesTableColumns}
                      rows={devices}
                      style={{
                          height: 500,
                          width: '100%',
                          marginTop: "1em"
                      }}/>            
        </>
    );
}