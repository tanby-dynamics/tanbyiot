import {DeveloperMode, Mail, MoveToInbox, OpenInNew, Dashboard } from "@mui/icons-material";
import {AppBar, Box,
    CssBaseline, Divider, Drawer, Link, List, ListItem, ListItemButton, ListItemIcon, ListItemText, Toolbar, Typography } from "@mui/material";
import { useLocation } from "react-router-dom";

const drawerWidth = 240;

export function AppTemplate(props: {children: any}) {
    var location = useLocation();
    
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
                            width: drawerWidth,
                            flexShrink: 0,
                            "& .MuiDrawer-paper": {
                                width: drawerWidth,
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
                            <ListItemButton href="/devices" selected={location.pathname === "/devices"}>
                                <ListItemIcon><DeveloperMode/></ListItemIcon>
                                <ListItemText primary={"Devices"}/>
                            </ListItemButton>
                        </ListItem>
                    </List>
                    <Divider/>
                    <List>
                        <ListItem disablePadding>
                            <ListItemButton>
                                <ListItemIcon><MoveToInbox/></ListItemIcon>
                                <ListItemText primary={"Thing 3"}/>
                            </ListItemButton>
                        </ListItem>
                        <ListItem disablePadding>
                            <ListItemButton>
                                <ListItemIcon><Mail/></ListItemIcon>
                                <ListItemText primary={"Thing 4"}/>
                            </ListItemButton>
                        </ListItem>
                    </List>
                    <Divider/>
                    <List>
                        <ListItem disablePadding>
                            <ListItemButton href={"https://tanbydynamics.co"} target={"_blank"}>
                                <small>Tanby Dynamics <OpenInNew sx={{ width: 12, height: 12 }}/></small>
                            </ListItemButton>
                        </ListItem>
                        <ListItem disablePadding>
                            <ListItemButton disabled>
                                <small>0.1.0</small>
                            </ListItemButton>
                        </ListItem>
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