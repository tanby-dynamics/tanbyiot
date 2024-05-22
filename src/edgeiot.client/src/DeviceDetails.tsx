import {Alert, Box,
    Breadcrumbs,
    LinearProgress, Link, Paper, Tab, Table, TableBody, TableCell, TableContainer, TableRow, Tabs,
    Typography} from "@mui/material";
import { useState } from "react";
import {useParams } from "react-router-dom";
import {DeviceTelemetry} from "./components/devices/DeviceTelemetry.tsx";
import {DeviceInstructions} from "./components/devices/DeviceInstructions.tsx";
import { useQuery } from "@tanstack/react-query";
import {getDevice} from "./api/DevicesApi.ts";
import {CopyValueButton} from "./components/shared/CopyValueButton.tsx";
import {formatRelativeTimestamp} from "./helpers/formatting.ts";

export function DeviceDetails() {
    const {
        id: deviceId
    } = useParams<{ id: string }>();
    const [ selectedTab, setSelectedTab ] = useState(0);

    if (deviceId === undefined) {
        return <Alert severity={"error"}>No device ID provided in path</Alert>;
    }   
    
    const {
        isPending,
        isError,
        error,
        data: device
    } = useQuery({
        queryKey: ["device-history-details"],
        queryFn: () => getDevice(deviceId)
    });
    
    return (
        <>
            {isPending && <LinearProgress/>}
            {isError && (
                <Alert severity={"error"}>Error getting device details: {error.name}, {error.message}</Alert>
            )}
            {device && (
                <>
                    <Breadcrumbs aria-label={"Breadcrumbs"}>
                        <Link underline={"hover"} color={"inherit"} href={"/devices"}>Devices</Link>
                        <Typography color={"text.primary"}>{device.name}</Typography>
                    </Breadcrumbs>
                    
                    <TableContainer sx={{ width: 500, marginTop: "1em" }} component={Paper}>
                        <Table size={"small"} aria-label={"Device details"}>
                            <TableBody>
                                <TableRow>
                                    <TableCell>Tenant ID</TableCell>
                                    <TableCell><code>{"de37f1e6-70a1-4c69-bdbc-317ff86b5267"}</code> <CopyValueButton value={"de37f1e6-70a1-4c69-bdbc-317ff86b5267"}/></TableCell>
                                </TableRow>
                                <TableRow>
                                    <TableCell>Device ID</TableCell>
                                    <TableCell><code>{device.id}</code> <CopyValueButton value={device.id}/></TableCell>
                                </TableRow>
                                <TableRow>
                                    <TableCell>Group name</TableCell>
                                    <TableCell>
                                        {!device.groupName && <code>---</code>}
                                        {device.groupName && (
                                            <code>
                                                {device.groupName} <CopyValueButton value={device.groupName}/>
                                            </code>)}
                                    </TableCell>
                                </TableRow>
                                <TableRow>
                                    <TableCell>Last connected</TableCell>
                                    <TableCell>
                                        {formatRelativeTimestamp(device.lastConnected, "Never")}
                                    </TableCell>
                                </TableRow>
                            </TableBody>
                        </Table>
                    </TableContainer> 
                </>
            )}
            <Tabs value={selectedTab}
                  onChange={(_, newValue) => setSelectedTab(newValue)}
                  aria-label={"Device history"}
                  sx={{ borderBottom: 1, borderColor: "divider", paddingTop: "1em" }}>
                <Tab label={"Telemetry"} id={"telemetry"} aria-controls={"telemetry-tabpanel"}/>
                <Tab label={"Instructions"} id={"instructionsTab"} aria-controls={"instructions-tabpanel"}/>
            </Tabs>
            <div role={"tabpanel"}
                 hidden={selectedTab !== 0}
                 aria-labelledby={"telemetry-tabpanel"}
                 style={{paddingTop: "1em"}}>
                {selectedTab === 0 && (
                    <DeviceTelemetry deviceId={deviceId}/>
                )}
            </div>
            <div role={"tabpanel"}
                 hidden={selectedTab !== 1}
                 aria-labelledby={"instructions-tabpanel"}
                 style={{paddingTop: "1em"}}>
                {selectedTab === 1 && (
                    <DeviceInstructions deviceId={deviceId}/>
                )}
            </div>
        </>
    );
}