import {Breadcrumbs, Typography } from "@mui/material";
import { useQuery } from "@tanstack/react-query";
import { Helmet } from "react-helmet";
import {QueryKeys} from "../../api/constants.ts";
import {useAdminUsersApi} from "../../api/AdminUsersApi.ts";
import {SystemUser} from "../../api/types.t.ts";
import {DataGrid, GridColDef } from "@mui/x-data-grid";
import {CopyValueButton} from "../../components/shared/CopyValueButton.tsx";

export function AdminUsers() {
    const adminUsersApi = useAdminUsersApi();
    const {
        data: users
    } = useQuery({
        queryKey: [QueryKeys.AdminUsers],
        queryFn: adminUsersApi.getUsers
    });
    
    const usersTableColumns: GridColDef<SystemUser>[] = [
        {
            field: "email",
            headerName: "Email address",
            flex: 1
        },
        {
            field: "currentTenant",
            headerName: "Current Tenant",
            width: 150,
            valueGetter: (_, user) => user.currentTenant?.name ?? "None"
        },
        {
            field: "id",
            headerName: "ID",
            flex: 1,
            renderCell: (params) => (
                <>
                    <code>{params.row.id}</code>
                    <CopyValueButton value={params.row.id} tooltip={"Copy user ID"}/>
                </>
            )
        },
        {
            field: "externalId",
            headerName: "External ID",
            flex: 1,
            renderCell: (params) => (
                <>
                    <code>{params.row.externalId}</code>
                    <CopyValueButton value={params.row.externalId} tooltip={"Copy external ID"}/>
                </>
            )
        },        
        {
            field: "tenantCount",
            headerName: "Tenants",
            width: 100,
            valueGetter: (_, user) => user.tenants.length
        }
    ];
    
    return (
        <>
            <Helmet>
                <title>Users - System admin - tanbyiot.app</title>
            </Helmet>
            <Breadcrumbs>
                <Typography color="text.primary">Users (System admin)</Typography>
            </Breadcrumbs>
            
            <DataGrid columns={usersTableColumns}
                      rows={users ?? []}
                      autoHeight
                      style={{
                          width: "100%",
                          marginTop: "1em"
                      }}/>
        </>
    );
}
