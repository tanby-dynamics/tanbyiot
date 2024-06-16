import {Breadcrumbs, Typography } from "@mui/material";
import { Helmet } from "react-helmet";

export function AdminOverview() {
    return (
        <>
            <Helmet>
                <title>Overview - System admin - tanbyiot.app</title>
            </Helmet>
            <Breadcrumbs>
                <Typography color="text.primary">Overview (System admin)</Typography>
            </Breadcrumbs>
        </>
    );
}
