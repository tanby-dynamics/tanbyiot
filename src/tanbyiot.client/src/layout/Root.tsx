import {AppBar, Box, Link, Typography, Toolbar } from "@mui/material";
import {MainMenu} from "./MainMenu.tsx";
import { Outlet, useNavigate } from "react-router-dom";
import {useUsersApi} from "../api/UsersApi.ts";
import { useQuery } from "@tanstack/react-query";
import { useAuth0 } from "@auth0/auth0-react";
import {QueryKeys} from "../api/constants.ts";
import { useEffect } from "react";

export function Root() {
    const navigate = useNavigate();
    const usersApi = useUsersApi();
    const {
        isLoading: isAuth0UserLoading,
        isAuthenticated,
        loginWithRedirect,
    } = useAuth0();
    const {
        data: user
    } = useQuery({
        queryKey: [QueryKeys.User],
        queryFn: usersApi.getCurrentUser,
        enabled: isAuthenticated
    });
    
    if (!isAuth0UserLoading && !isAuthenticated) {
        loginWithRedirect();
    }
    
    // If there are no tenants, start the onboarding process
    useEffect(() => {
        if (user && user.tenants.length === 0 && location.pathname !== "/onboarding/select-subscription-level") {
            navigate("/onboarding/select-subscription-level");
        }        
    }, [user, location])

    return (
        <>
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

                {user && user.currentTenant && <MainMenu/>}

                <Box component={"main"}
                     sx={{ flexGrow: 1, p: 3 }}>
                    <Toolbar/>
                    <Outlet/>
                </Box>
            </Box>
        </>
    );
}