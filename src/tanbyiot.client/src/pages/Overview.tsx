import {Breadcrumbs, Typography } from "@mui/material";
import {Helmet} from "react-helmet";

export function Overview() {
    return (
        <>
            <Helmet>
                <title>Overview - tanbyiot.app</title>
            </Helmet>
            <Breadcrumbs>
                <Typography color="text.primary">Overview</Typography>
            </Breadcrumbs>
        </>
    );
}