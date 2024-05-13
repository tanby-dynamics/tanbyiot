import {Mail, MoveToInbox } from "@mui/icons-material";
import {AppBar, Box,
    CssBaseline, Divider, Drawer, List, ListItem, ListItemButton, ListItemIcon, ListItemText, Toolbar, Typography } from "@mui/material";

const drawerWidth = 240;

export function AppTemplate(props: {children: any}) {
    return (
        <>
            <CssBaseline/>
            <Box sx={{ display: 'flex' }}>
                <AppBar position={"fixed"}
                        sx={{ zIndex: (theme) => theme.zIndex.drawer + 1 }}>
                    <Toolbar>
                        <Typography variant={"h6"} noWrap component={"div"}>
                            Edge IoT
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
                            <ListItemButton href={"/"}>
                                <ListItemIcon><MoveToInbox/></ListItemIcon>
                                <ListItemText primary={"Dashboard"}/>
                            </ListItemButton>
                        </ListItem>
                        <ListItem disablePadding>
                            <ListItemButton href="/devices" selected={true}> {/* TODO hook up to route */}
                                <ListItemIcon><Mail/></ListItemIcon>
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