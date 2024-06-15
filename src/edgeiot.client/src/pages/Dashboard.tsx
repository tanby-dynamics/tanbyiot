import { useAuth0 } from "@auth0/auth0-react";
import { Typography } from "@mui/material";
import {Helmet} from "react-helmet";

export function Dashboard() {
    const {
        isAuthenticated
    } = useAuth0();
    
    return (
        <>
            <Helmet>
                <title>Dashboard - edgeiot</title>
            </Helmet>
            <Typography paragraph>
                This is the dashboard
            </Typography>

            {!isAuthenticated && (
                <>
                    Sign in or sign up
                </>
            )}
        </>
    );
}