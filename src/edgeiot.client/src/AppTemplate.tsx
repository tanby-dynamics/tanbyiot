import {DeveloperMode, OpenInNew, Dashboard, Gavel, Settings, ManageAccounts } from "@mui/icons-material";
import {AppBar, Box,
    CssBaseline, Divider, Drawer, Link, List, ListItem, ListItemButton, ListItemIcon, ListItemText, Toolbar,
    Tooltip, Typography } from "@mui/material";
import { useQuery } from "@tanstack/react-query";
import { useLocation } from "react-router-dom";
import {getVersion} from "./api/Api.ts";
import { formatTimestamp } from "./helpers/formatting.ts";
import moment from "moment";
import { useEffect, useState } from "react";

export function AppTemplate(props: {children: any}) {
    const location = useLocation();
    const [ nowTimestamp, setNowTimestamp ] = useState(moment);
    
    // Refresh nowTimestamp every second
    useEffect(() => {
        const timer = setInterval(() => setNowTimestamp(moment()), 1000);
        
        return () => clearInterval(timer);
    }, []);
    
    const {
        isError: isVersionError,
        error: versionError,
        data: version
    } = useQuery({
        queryKey: ["version"],
        queryFn: getVersion
    });
    
    return (
        <>
            <CssBaseline/>
            <Box sx={{ display: 'flex' }}>
                <AppBar position={"fixed"}
                        sx={{ zIndex: (theme) => theme.zIndex.drawer + 1 }}>
                    <Toolbar>
                        <Typography variant={"h6"} noWrap component={"div"}>
                            <Link color={"inherit"} underline={"none"} href={"/"}>
                                edgeiot portal
                            </Link>
                        </Typography>
                    </Toolbar>
                </AppBar>
                
                <Drawer variant={"permanent"}
                        anchor={"left"}
                        sx={{
                            width: 240,
                            flexShrink: 0,
                            "& .MuiDrawer-paper": {
                                width: 240,
                                boxSizing: "border-box"
                            }
                        }}>
                    <Toolbar/>
                    <List>
                        <ListItem disablePadding>
                            <ListItemButton href={"/"} selected={location.pathname === "/"}>
                                <ListItemIcon><Dashboard/></ListItemIcon>
                                <ListItemText primary={"Dashboard"}/>
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
                    </List>
                    <Divider/>
                    <List>
                        <ListItem disablePadding>
                            <ListItemButton href="/manage-tenant" selected={location.pathname.startsWith("/manage-tenant")}>
                                <ListItemIcon><Settings/></ListItemIcon>
                                <ListItemText primary={"Manage tenant"}/>
                            </ListItemButton>
                        </ListItem>
                        <ListItem disablePadding>
                            <ListItemButton href="/manage-account" selected={location.pathname === "/manage-account"}>
                                <ListItemIcon><ManageAccounts/></ListItemIcon>
                                <ListItemText primary={"Account settings"}/>
                            </ListItemButton>
                        </ListItem>
                    </List>
                    <Divider/>
                    <Typography variant={"subtitle2"} style={{ padding: "1em"}}>
                        <small>
                            Tenant:{" "}
                            <strong>
                                <Tooltip title={"Manage tenant"}>
                                    <Link underline={"hover"} href={"/manage-tenant"}>
                                        The Greenhaus
                                    </Link>
                                </Tooltip>
                            </strong>
                            <br/>
                            User:{" "}
                            <strong>
                                <Tooltip title={"Manage account settings"}>
                                    <Link underline={"hover"} href={"/manage-account"}>
                                        admin@thegreenhaus.com.au
                                    </Link>
                                </Tooltip>                                
                            </strong>
                        </small>
                    </Typography>
                    <Divider/>
                    <List>
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
                        <ListItem disablePadding>
                            <ListItemButton disabled>
                                <small>
                                    {isVersionError && `Error getting version: ${versionError.name}, ${versionError.message}`}
                                    {version && `v${version}`}<br/>
                                    {formatTimestamp(nowTimestamp)}<br/>
                                    &copy; {moment().format("yyyy")} Tanby Dynamics
                                </small>
                            </ListItemButton>
                        </ListItem>

                        {/* Only show dev tools if the API base URL hasn't been set */}
                        {!import.meta.env.API_BASE_URL && (
                            <>
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
                            </>
                        )}
                    </List>
                </Drawer>
                
                <Box component={"main"}
                     sx={{ flexGrow: 1, p: 3 }}>
                    <Toolbar/>
                    {props.children}
                </Box>
            </Box>
        </>
    );
}