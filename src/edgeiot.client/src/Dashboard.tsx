import { Typography } from "@mui/material";
import {Helmet} from "react-helmet";

export function Dashboard() {
    return (
        <>
            <Helmet>
                <title>Dashboard - edgeiot</title>
            </Helmet>
            <Typography paragraph>
                This is the dashboard
            </Typography>
        </>
    );
}