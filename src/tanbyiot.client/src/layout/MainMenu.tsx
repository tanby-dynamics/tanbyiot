import {Divider, Drawer, Link, List, ListItem, ListItemButton, ListItemIcon, ListItemText, Tooltip, Typography } from "@mui/material";
import {formatTimestamp} from "../helpers/formatting.ts";
import {Dashboard, Gavel, DeveloperMode, OpenInNew, Settings, ManageAccounts, People } from "@mui/icons-material";
import {getVersion} from "../api/Api.ts";
import { useEffect, useState } from "react";
import { useQuery } from "@tanstack/react-query";
import moment from "moment";
import { useAuth0 } from "@auth0/auth0-react";
import {usePermissions, useUser} from "../api/UsersApi.ts";
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
    const {
        logout,
        user: auth0User
    } = useAuth0();
    const user = useUser();
    const {
        isSystemAdmin
    } = usePermissions();

    // Refresh nowTimestamp every second
    useEffect(() => {
        const timer = setInterval(() => setNowTimestamp(moment()), 1000);

        return () => clearInterval(timer);
    }, []);

    function signOut() {
        logout({
            logoutParams: {
                returnTo: window.location.origin
            }
        });
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
            {/* System admin */}
            {isSystemAdmin && (
                <>
                    <Typography variant={"subtitle2"} style={{ padding: "1em"}}>
                        <small>
                            System admin
                        </small>
                    </Typography>
                    <List>
                        <ListItem disablePadding>
                            <ListItemButton href={"/admin/overview"} selected={location.pathname === "/admin/overview"}>
                                <ListItemIcon><Dashboard/></ListItemIcon>
                                <ListItemText primary={"Overview"}/>
                            </ListItemButton>
                        </ListItem>
                        <ListItem disablePadding>
                            <ListItemButton href={"/admin/tenants"} selected={location.pathname.startsWith("/admin/tenants")}>
                                <ListItemIcon><People/></ListItemIcon>
                                <ListItemText primary={"Tenants"}/>
                            </ListItemButton>
                        </ListItem>
                        <ListItem disablePadding>
                            <ListItemButton href={"/admin/users"} selected={location.pathname.startsWith("/admin/users")}>
                                <ListItemIcon><People/></ListItemIcon>
                                <ListItemText primary={"Users"}/>
                            </ListItemButton>
                        </ListItem>
                    </List>       
                </>
            )}

            {/* Select tenant */}
            <Divider/>
            <List>
                <ListItem disablePadding>
                    <ListItemButton href={"/tenants"} selected={location.pathname.startsWith("/tenants")}>
                        <ListItemIcon><People/></ListItemIcon>
                        <ListItemText primary={"Tenants"}/>
                    </ListItemButton>
                </ListItem>
            </List>

            {/* Management pages */}
            <Divider/>
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
            </List>

            {/* Tenant and account management */}
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
                <ListItem disablePadding>
                    <ListItemButton onClick={signOut}>
                        <ListItemIcon><ManageAccounts/></ListItemIcon>
                        <ListItemText primary={"Sign out"}/>
                    </ListItemButton>
                </ListItem>
            </List>

            {/* Tenant and account details */}
            <Divider/>
            <Typography variant={"subtitle2"} style={{ padding: "1em"}}>
                <small>
                    Tenant:{" "}
                    <strong>
                        <Tooltip title={"Manage tenant"}>
                            <Link underline={"hover"} href={"/manage-tenant"}>
                                {user?.currentTenant?.name}
                            </Link>
                        </Tooltip>
                    </strong>
                    <br/>
                    User:{" "}
                    <strong>
                        <Tooltip title={"Manage account settings"}>
                            <Link underline={"hover"} href={"/manage-account"}>
                                {auth0User?.email}
                            </Link>
                        </Tooltip>
                    </strong>
                </small>
            </Typography>

            {/* Links and dev tools */}
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

                {/* Only show dev tools if this is a local dev environment */}
                {import.meta.env.DEV && (
                    <>
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
                            <ListItemButton href={"https://manage.auth0.com/dashboard/au/dev-edgeiot/"} target={"_blank"}>
                                <small>Auth0 <OpenInNew sx={{ width: 12, height: 12 }}/></small>
                            </ListItemButton>
                        </ListItem>
                        <ListItem disablePadding>
                            <ListItemButton href={"https://mui.com"} target={"_blank"}>
                                <small>MUI <OpenInNew sx={{ width: 12, height: 12 }}/></small>
                            </ListItemButton>
                        </ListItem>
                    </>
                )}
            </List>
        </Drawer>
    );
}