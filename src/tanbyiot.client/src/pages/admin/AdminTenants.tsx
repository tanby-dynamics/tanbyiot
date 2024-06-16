import {Breadcrumbs, Typography } from "@mui/material";
import { Helmet } from "react-helmet";

export function AdminTenants() {
    return (
        <>
            <Helmet>
                <title>Tenants - System admin - tanbyiot.app</title>
            </Helmet>
            <Breadcrumbs>
                <Typography color="text.primary">Tenants (System admin)</Typography>
            </Breadcrumbs>
        </>
    );
}
