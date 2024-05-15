import {Alert, Button, LinearProgress, Typography } from "@mui/material";
import { useQuery, useQueryClient } from "@tanstack/react-query";
import {getAllDevices} from "./api/DevicesApi.ts";
import { AddCircleOutlined } from "@mui/icons-material";

export function Devices() {
    //const queryClient = useQueryClient();
    
    const { 
        isPending, 
        isError, 
        error, 
        data: devices 
    } = useQuery({
        queryKey: ["devices"],
        queryFn: getAllDevices
    })
    
    /*
    setInterval(() => queryClient.invalidateQueries({
        queryKey: ["devices"]
    }), 5000);
    */ 
    
    if (isError) {
        return <Alert severity={"error"}>Error getting devices: {error.name}, {error.message}</Alert>
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
            {/* TODO: implement me! open modal to set up a new device */}
            <Button variant={"contained"} startIcon={<AddCircleOutlined/>}>
                New device
            </Button>
            {/* TODO proper table for devices */}
            {devices && devices.length > 0 && devices.map((device) => (
                <p>{device.id}</p>
            ))}
        </>
    );
}