import {DeveloperMode, OpenInNew, Dashboard, Gavel, Settings, ManageAccounts } from "@mui/icons-material";
import {AppBar, Box,
    Divider, Drawer, Link, List, ListItem, ListItemButton, ListItemIcon, ListItemText, Toolbar,
    Tooltip, Typography } from "@mui/material";
import { useQuery } from "@tanstack/react-query";
import { useLocation } from "react-router-dom";
import {getVersion} from "./api/Api.ts";
import { formatTimestamp } from "./helpers/formatting.ts";
import moment from "moment";
import { useEffect, useState } from "react";
import { useAuth0 } from "@auth0/auth0-react";

export function AppTemplate(props: {children: any}) {
    const location = useLocation();
    const [ nowTimestamp, setNowTimestamp ] = useState(moment);
    const {
        loginWithRedirect,
        logout,
        isAuthenticated,
        user,
        isLoading: isAuthenticationLoading
    } = useAuth0();
    
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

    if (isAuthenticationLoading) {
        return null;
    }
        
    function signIn() {
        loginWithRedirect();
    }
    
    function signOut() {
        logout({
            logoutParams: {
                returnTo: window.location.origin
            }
        });
    }
    
    return (
        <>
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
                                boxSizing: "border-box",
                                paddingTop: "64px"
                            }
                        }}>
                    {/* Sign in */}
                    {!isAuthenticated && (
                        <>
                            <Divider/>
                            <List disablePadding>
                                <ListItem disablePadding>
                                    <ListItemButton onClick={signIn}>
                                        <ListItemIcon><ManageAccounts/></ListItemIcon>
                                        <ListItemText primary={"Sign in"}/>
                                    </ListItemButton>
                                </ListItem>
                            </List>
                        </>
                    )}

                    {/* Select tenant */}
                    {isAuthenticated && (
                        <>
                            <List disablePadding>
                                <ListItem disablePadding>
                                    <ListItemButton>select tenant</ListItemButton>
                                </ListItem>
                            </List>
                        </>
                    )}

                    {/* Management pages */}
                    {isAuthenticated && (
                        <>
                            <Divider/>
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
                        </>
                    )}

                    {/* Tenant and account management */}
                    {isAuthenticated && (
                        <>
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
                                {isAuthenticated && (
                                    <ListItem disablePadding>
                                        <ListItemButton onClick={signOut}>
                                            <ListItemIcon><ManageAccounts/></ListItemIcon>
                                            <ListItemText primary={"Sign out"}/>
                                        </ListItemButton>
                                    </ListItem>
                                )}
                            </List>
                        </>
                    )}

                    {/* Tenant and account details */}
                    {isAuthenticated && (
                        <>
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
                                    {user && (
                                        <>
                                            User:{" "}
                                            <strong>
                                                <Tooltip title={"Manage account settings"}>
                                                    <Link underline={"hover"} href={"/manage-account"}>
                                                        {user && user.email}
                                                        {!user && "signed out"}
                                                    </Link>
                                                </Tooltip>
                                            </strong>
                                        </>
                                    )}
                                    {!user && "Signed out"}
                                </small>
                            </Typography> 
                        </>
                    )}

                    {/* Links and dev tools */}
                    <Divider/>
                    <List>
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

                        {/* Only show dev tools if this is a local dev environment */}
                        {import.meta.env.DEV && (
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
                                <ListItem disablePadding>
                                    <ListItemButton href={"https://manage.auth0.com/dashboard/au/dev-edgeiot/"} target={"_blank"}>
                                        <small>Auth0 <OpenInNew sx={{ width: 12, height: 12 }}/></small>
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