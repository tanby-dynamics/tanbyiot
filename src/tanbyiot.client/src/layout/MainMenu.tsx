import {Divider, Drawer, List, ListItem, ListItemButton, ListItemIcon, ListItemText } from "@mui/material";
import {formatTimestamp} from "../helpers/formatting.ts";
import {Dashboard, Gavel, DeveloperMode, OpenInNew, Description } from "@mui/icons-material";
import {getVersion} from "../api/Api.ts";
import { useEffect, useState } from "react";
import { useQuery } from "@tanstack/react-query";
import moment from "moment";
import {QueryKeys} from "../api/constants.ts";

export function MainMenu() {
    const [ nowTimestamp, setNowTimestamp ] = useState(moment());
    const {
        isError: isVersionError,
        error: versionError,
        data: version
    } = useQuery({
        queryKey: [QueryKeys.Version],
        queryFn: getVersion
    });
    
    // Refresh nowTimestamp every second
    useEffect(() => {
        const timer = setInterval(() => setNowTimestamp(moment()), 1000);

        return () => clearInterval(timer);
    }, []);

    function sendTelemetry() {
        alert("TODO send telemetry");
    }
    
    function sendInstruction() {
        alert("TODO send instruction");
    }

    return (
        <Drawer variant={"permanent"}
                anchor={"left"}
                sx={{
                    width: 240,
                    flexShrink: 0,
                    "& .MuiDrawer-paper": {
                        width: 240,
                        boxSizing: "border-box",
                        paddingTop: "64px"
                    }
                }}>
            {/* Management pages */}
            <List>
                <ListItem disablePadding>
                    <ListItemButton href={"/"} selected={location.pathname === "/"}>
                        <ListItemIcon><Dashboard/></ListItemIcon>
                        <ListItemText primary={"Overview"}/>
                    </ListItemButton>
                </ListItem>
                <ListItem disablePadding>
                    <ListItemButton href="/devices" selected={ location.pathname.startsWith("/device")}>
                        <ListItemIcon><DeveloperMode/></ListItemIcon>
                        <ListItemText primary={"Devices"}/>
                    </ListItemButton>
                </ListItem>
                <ListItem disablePadding>
                    <ListItemButton href="/rules" selected={location.pathname === "/rules"}>
                        <ListItemIcon><Gavel/></ListItemIcon>
                        <ListItemText primary={"Rules"}/>
                    </ListItemButton>
                </ListItem>
                <ListItem disablePadding>
                    <ListItemButton href="/application-states" selected={location.pathname === "/application-states"}>
                        <ListItemIcon><Description/></ListItemIcon>
                        <ListItemText primary={"Application state"}/>
                    </ListItemButton>
                </ListItem>
            </List>
            
            <Divider/>
            <List>
                <ListItem disablePadding>
                    <ListItemButton onClick={sendTelemetry}>
                        <small>Send test telemetry</small>
                    </ListItemButton>
                </ListItem>
                <ListItem disablePadding>
                    <ListItemButton onClick={sendInstruction}>
                        <small>Send test instruction</small>
                    </ListItemButton>
                </ListItem>
            </List>

            {/* Only show dev links if this is a local dev environment */}
            {import.meta.env.DEV && (
                <>
                    <Divider/>
                    <List>
                        <ListItem disablePadding>
                            <ListItemButton href={"https://localhost:7061/swagger"} target={"_blank"}>
                                <small>Swagger <OpenInNew sx={{ width: 12, height: 12 }}/></small>
                            </ListItemButton>
                        </ListItem>
                        <ListItem disablePadding>
                            <ListItemButton href={"http://localhost:8025"} target={"_blank"}>
                                <small>Mailhog <OpenInNew sx={{ width: 12, height: 12 }}/></small>
                            </ListItemButton>
                        </ListItem>
                        <ListItem disablePadding>
                            <ListItemButton href={"http://localhost:5341"} target={"_blank"}>
                                <small>Seq <OpenInNew sx={{ width: 12, height: 12 }}/></small>
                            </ListItemButton>
                        </ListItem>
                        <ListItem disablePadding>
                            <ListItemButton href={"https://mui.com/material-ui/all-components/"} target={"_blank"}>
                                <small>MUI <OpenInNew sx={{ width: 12, height: 12 }}/></small>
                            </ListItemButton>
                        </ListItem>
                    </List>
                </>
            )}
            
            {/* Links */}
            <Divider/>
            <List>
                <ListItem disablePadding>
                    <ListItemButton disabled>
                        <small>
                            {isVersionError && `Error getting version: ${versionError.name}, ${versionError.message}`}
                            {!isVersionError && version && `v${version}`}<br/>
                            {formatTimestamp(nowTimestamp)}<br/>
                            &copy; {moment().format("yyyy")} Tanby Dynamics
                        </small>
                    </ListItemButton>
                </ListItem>
                <ListItem disablePadding>
                    <ListItemButton href={"https://docs.edgeiot.app"} target={"_blank"}>
                        <small>Documentation <OpenInNew sx={{ width: 12, height: 12 }}/></small>
                    </ListItemButton>
                </ListItem>
                <ListItem disablePadding>
                    <ListItemButton href={"https://tanbydynamics.co"} target={"_blank"}>
                        <small>Tanby Dynamics <OpenInNew sx={{ width: 12, height: 12 }}/></small>
                    </ListItemButton>
                </ListItem>
            </List>
        </Drawer>
    );
}