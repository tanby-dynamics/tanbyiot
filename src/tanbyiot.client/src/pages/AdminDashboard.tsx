import { Typography } from "@mui/material";
import { Helmet } from "react-helmet";
import { useQuery } from "@tanstack/react-query";
import {useAdminTenantsApi} from "../api/AdminTenantsApi.ts";

export function AdminDashboard() {
    const api = useAdminTenantsApi();
    const {
        data: tenants,
        error,
        isPending
    } = useQuery({
        queryKey: ["tenants"],
        queryFn: api.getAllTenants
    });
    
    return (
        <>
            <Helmet>
                <title>Admin dashboard - tanbyiot.app</title>
            </Helmet>
            <Typography paragraph>
                This is the admin dashboard
            </Typography>
            {isPending && <p>Loading</p>}
            {error && <pre>{error.message}</pre>}
            <ul>
                {tenants && tenants.map((tenant) => (
                    <li>{tenant}</li>
                ))}            
            </ul>
            
        </>
    );
}