import { Helmet } from "react-helmet";
import {useUser, useUsersApi} from "../api/UsersApi.ts";
import { useQueryClient } from "@tanstack/react-query";
import {Alert, Breadcrumbs, LinearProgress, Link, Tooltip, Typography } from "@mui/material";
import { DataGrid, GridActionsCellItem, GridColDef } from "@mui/x-data-grid";
import {SystemUser, Tenant} from "../api/types.t.ts";
import {CopyValueButton} from "../components/shared/CopyValueButton.tsx";
import { Edit } from "@mui/icons-material";
import { toast } from "react-toastify";
import { QueryKeys } from "../api/constants.ts";

export function Tenants() {
    const queryClient = useQueryClient();
    const user = useUser();
    const usersApi = useUsersApi();
    
    async function selectTenant(tenant: Tenant) {
        try {
            await usersApi.setCurrentTenant(tenant);
            queryClient.setQueryData([QueryKeys.User], (oldData: SystemUser) => {
                return {
                    ...oldData,
                    currentTenant: tenant
                };
            });
            toast.success(`Set current tenant to ${tenant.name}`);
        } catch (error) {
            console.error("Error setting current tenant", error);
            toast.error("Error setting current tenant");
        }
    }
    
    const tenantsTableColumns: GridColDef<Tenant>[] = [
        {
            field: "name",
            headerName: "Name",
            flex: 1,
            renderCell: (params) => (
                <>
                    {params.row.id === user!.currentTenant?.id && (
                        <strong>{params.row.name} (current)</strong>
                    )}
                    {params.row.id !== user!.currentTenant?.id && (
                        <>
                            {params.row.name}
                            {" "}(<Link component={"button"} onClick={() => selectTenant(params.row)}>select</Link>)
                        </>
                    )}
                </>
            ) 
        },
        {
            field: "subscriptionLevel",
            headerName: "Subscription level",
            flex: 0.5
        },
        {
            field: "id",
            headerName: "ID",
            width: 350,
            renderCell: (params) => (
                <>
                    <code>{params.row.id}</code>
                    <CopyValueButton value={params.row.id} tooltip={"Copy tenant ID"}/>
                </>
            )
        },
        {
            field: "actions",
            headerName: "Actions",
            type: "actions",
            getActions: (params) => [
                <GridActionsCellItem icon={<Tooltip title={"Edit tenant"}><Edit/></Tooltip>}
                                     label={"Edit tenant"}
                                     onClick={() => alert(`Edit ${params.row.id}`)}/>
            ]
        }
    ];
    
    if (!user) {
        return <LinearProgress/>;
    }
    
    return (
        <>
            <Helmet>
                <title>Tenants - tanbyiot.app</title>
            </Helmet>
            <Breadcrumbs aria-label={"Breadcrumbs"}>
                <Typography color={"text.primary"}>Tenants</Typography>
            </Breadcrumbs>
            
            {user.tenants.length === 0 && (
                <Alert severity={"warning"}>You have no tenants. You should create one now!</Alert>
            )}
            
            <DataGrid columns={tenantsTableColumns}
                      rows={user.tenants}
                      autoHeight
                      style={{
                          width: "100%",
                          marginTop: "1em"
                      }}/>
        </>
    );
}