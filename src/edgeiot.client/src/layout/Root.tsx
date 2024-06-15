import {AppBar, Box, Link, Typography, Toolbar } from "@mui/material";
import {MainMenu} from "./MainMenu.tsx";
import { Outlet } from "react-router-dom";
import {OnboardingWorkflow} from "./OnboardingWorkflow.tsx";

export function Root() {
    return (
        <>
            <OnboardingWorkflow>
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

                    <MainMenu/>

                    <Box component={"main"}
                         sx={{ flexGrow: 1, p: 3 }}>
                        <Toolbar/>
                        <Outlet/>
                    </Box>
                </Box>                
            </OnboardingWorkflow>
        </>
    );
}