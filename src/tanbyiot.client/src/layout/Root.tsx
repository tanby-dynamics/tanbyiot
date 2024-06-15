import {AppBar, Box, Link, Typography, Toolbar } from "@mui/material";
import {MainMenu} from "./MainMenu.tsx";
import { Outlet } from "react-router-dom";
import {OnboardingWorkflow} from "./OnboardingWorkflow.tsx";
import {useUsersApi} from "../api/UsersApi.ts";
import { useQuery } from "@tanstack/react-query";

export function Root() {
    const usersApi = useUsersApi();
    const {
        data: user
    } = useQuery({
        queryKey: ["user"],
        queryFn: usersApi.getCurrentUser
    });
    
    return (
        <>
            <OnboardingWorkflow>
                <Box sx={{ display: 'flex' }}>
                    <AppBar position={"fixed"}
                            sx={{ zIndex: (theme) => theme.zIndex.drawer + 1 }}>
                        <Toolbar>
                            <Typography variant={"h6"} noWrap component={"div"}>
                                <Link color={"inherit"} underline={"none"} href={"/"}>
                                    tanbyiot.app
                                    {user && user.currentTenant && ` - ${user.currentTenant.name}`}
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