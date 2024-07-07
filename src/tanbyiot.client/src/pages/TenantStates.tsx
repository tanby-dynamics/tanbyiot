import {Alert, Breadcrumbs, Button, CircularProgress, LinearProgress, Tooltip, Typography } from "@mui/material";
import { Helmet } from "react-helmet";
import {QueryKeys} from "../api/constants.ts";
import {useUser} from "../api/UsersApi.ts";
import { useQuery, useQueryClient } from "@tanstack/react-query";
import {DataGrid, GridActionsCellItem, GridColDef } from "@mui/x-data-grid";
import {useTenantStatesApi} from "../api/TenantStatesApi.ts";
import {AddTenantStateArgs, TenantState} from "../api/types.t.ts";
import {formatRelativeTimestamp} from "../helpers/formatting.ts";
import {AddCircleOutlined, Delete, Edit } from "@mui/icons-material";
import { toast } from "react-toastify";
import { useState } from "react";
import {EditTenantStateDialog} from "../components/tenant-states/EditTenantStateDialog.tsx";
import {AddTenantStateDialog} from "../components/tenant-states/AddTenantStateDialog.tsx";

export function TenantStates() {
    const tenantStatesApi = useTenantStatesApi();
    const user = useUser();
    const queryClient = useQueryClient();
    const [ isEditingState, setIsEditingState ] = useState(false);
    const [ stateBeingEdited, setStateBeingEdited ] = useState<TenantState | null>(null);
    const [ openAddTenantStateDialog, setOpenAddTenantStateDialog ] = useState(false);
    const [ addTenantStateError, setAddTenantStateError ] = useState<Error | null>();
    const [ isAddingTenantState, setIsAddingTenantState ] = useState(false);
    
    const {
        data: states,
        isError,
        isPending,
        error
    } = useQuery({
        queryKey: [QueryKeys.TenantStates],
        queryFn: tenantStatesApi.getAllTenantStates,
        refetchInterval: 10000,
        enabled: !!user?.currentTenant
    });
    
    const tenantStatesTableColumns: GridColDef<TenantState>[] = [
        {
            field: "key",
            headerName: "Key",
            flex: 0.5
        },
        {
            field: "value",
            headerName: "Value",
            flex: 0.5
        },
        {
            field: "setAt",
            headerName: "Set at",
            flex: 1,
            valueGetter: (_, state) => formatRelativeTimestamp(state.setAt)
        },
        {
            field: "actions",
            headerName: "Actions",
            type: "actions",
            getActions: (params) => [
                <GridActionsCellItem icon={<Tooltip title={"Edit value"}><Edit/></Tooltip>}
                                     label={"Edit value"}
                                     onClick={() => editState(params.row)}/>,
                <GridActionsCellItem icon={<Tooltip title={"Delete value"}><Delete/></Tooltip>}
                                     label={"Delete value"}
                                     onClick={() => deleteState(params.row)}/>
                
            ]
        }
    ];
    
    function editState(state: TenantState) {
        setIsEditingState(true);
        setStateBeingEdited(state);
    }
    
    async function onStateUpdated() {
        setIsEditingState(false);
        setStateBeingEdited(null);
        await refresh();
    }
    
    async function deleteState(state: TenantState) {
        if (!confirm("Are you sure you want to delete this state value?")) {
            return;
        }
        
        await tenantStatesApi.deleteTenantState(state.id);
        toast.success("Deleted state value");
        await refresh();
    }
    
    async function refresh() {
        await queryClient.invalidateQueries({
            queryKey: [QueryKeys.TenantStates]
        });
    }
    
    async function addTenantState(args: AddTenantStateArgs) {
        setIsAddingTenantState(true);
        setAddTenantStateError(null);
        
        try {
            await tenantStatesApi.addTenantState(args);
            await refresh();            
        } catch (error) {
            setAddTenantStateError(error as Error);
            console.error("Error adding tenant state", error);
            toast.error("Error adding tenant state");
        }
        
        setIsAddingTenantState(false);
    }
    
    if (isError) {
        return <Alert severity={"error"}>Error getting tenant states: {error.name}, {error.message}</Alert>;
    }
    
    if (isPending) {
        return <LinearProgress/>
    }
    
    return (
        <>
            <Helmet>
                <title>Tenant state - tanbyiot.app</title>
            </Helmet>
            <Breadcrumbs>
                <Typography color={"text.primary"}>Tenant state</Typography>
            </Breadcrumbs>

            {states && states.length === 0 && (
                <Alert severity={"warning"} style={{ marginBottom: "1em" }}>
                    Your tenant has no states defined.
                </Alert>
            )}

            <Typography align={"right"} style={{ paddingBottom: "1em" }}>
                <Button variant={"contained"}
                        startIcon={<AddCircleOutlined/>}
                        onClick={() => setOpenAddTenantStateDialog(true)}>
                    New state
                </Button>
            </Typography>
            <AddTenantStateDialog open={openAddTenantStateDialog}
                             onClose={() => setOpenAddTenantStateDialog(false)}
                             onSubmit={addTenantState}/>
            {isAddingTenantState && <CircularProgress/>}
            {addTenantStateError && (
                <Alert severity={"error"}
                       style={{ marginBottom: "1em" }}>
                    Error adding tenant state: {addTenantStateError.name}, {addTenantStateError.message}
                </Alert>
            )}
            <EditTenantStateDialog open={isEditingState}
                                   state={stateBeingEdited}
                                   onClose={() => setIsEditingState(false)}
                                   onSubmit={onStateUpdated}/>
            
            <Typography align={"right"}>
                <Button onClick={refresh}>Refresh</Button>
            </Typography>
            <Typography align={"right"} variant={"subtitle2"}>
                Automatically refreshes every 10 seconds
            </Typography>

            <DataGrid columns={tenantStatesTableColumns}
                      rows={states}
                      autoHeight
                      style={{
                          width: '100%',
                          marginTop: "1em"
                      }}/>
        </>
    );
}