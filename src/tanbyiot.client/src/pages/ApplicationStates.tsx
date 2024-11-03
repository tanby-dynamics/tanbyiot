import {Alert, Breadcrumbs, Button, CircularProgress, LinearProgress, Tooltip, Typography } from "@mui/material";
import { Helmet } from "react-helmet";
import {QueryKeys} from "../api/constants.ts";
import { useQuery, useQueryClient } from "@tanstack/react-query";
import {DataGrid, GridActionsCellItem, GridColDef } from "@mui/x-data-grid";
import {useApplicationStatesApi} from "../api/ApplicationStatesApi.ts";
import {AddApplicationStateArgs, ApplicationState} from "../api/types.t.ts";
import {formatRelativeTimestamp} from "../helpers/formatting.ts";
import {AddCircleOutlined, Delete, Edit } from "@mui/icons-material";
import { toast } from "react-toastify";
import { useState } from "react";
import {AddApplicationStateDialog} from "../components/application-states/AddApplicationStateDialog.tsx";
import {EditApplicationStateDialog} from "../components/application-states/EditApplicationStateDialog.tsx";

export function ApplicationStates() {
    const applicationStatesApi = useApplicationStatesApi();
    const queryClient = useQueryClient();
    const [ isEditingState, setIsEditingState ] = useState(false);
    const [ stateBeingEdited, setStateBeingEdited ] = useState<ApplicationState | null>(null);
    const [ openAddApplicationStateDialog, setOpenAddApplicationStateDialog ] = useState(false);
    const [ addApplicationStateError, setAddApplicationStateError ] = useState<Error | null>();
    const [ isAddingApplicationState, setIsAddingApplicationState ] = useState(false);
    
    const {
        data: states,
        isError,
        isPending,
        error
    } = useQuery({
        queryKey: [QueryKeys.ApplicationStates],
        queryFn: applicationStatesApi.getAllApplicationStates,
        refetchInterval: 10000
    });
    
    const applicationStatesTableColumns: GridColDef<ApplicationState>[] = [
        {
            field: "key",
            headerName: "Key",
            flex: 0.5,
            renderCell: (params) => <>
                <a href={""} 
                   onClick={(e) => { 
                       e.preventDefault(); 
                       editState(params.row); 
                   }}>
                    {params.row.key}
                </a>
            </>
        },
        {
            field: "value",
            headerName: "Value/payload",
            flex: 0.5
        },
        {
            field: "setAt",
            headerName: "Last updated",
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
    
    function editState(state: ApplicationState) {
        setIsEditingState(true);
        setStateBeingEdited(state);
    }
    
    async function onStateUpdated() {
        setIsEditingState(false);
        setStateBeingEdited(null);
        await refresh();
    }
    
    async function deleteState(state: ApplicationState) {
        if (!confirm("Are you sure you want to delete this state value?")) {
            return;
        }
        
        await applicationStatesApi.deleteApplicationState(state.id);
        toast.success("Deleted state value");
        await refresh();
    }
    
    async function refresh() {
        await queryClient.invalidateQueries({
            queryKey: [QueryKeys.ApplicationStates]
        });
    }
    
    async function addApplicationState(args: AddApplicationStateArgs) {
        setIsAddingApplicationState(true);
        setAddApplicationStateError(null);
        
        try {
            await applicationStatesApi.addApplicationState(args);
            await refresh();            
        } catch (error) {
            setAddApplicationStateError(error as Error);
            console.error("Error adding application state", error);
            toast.error("Error adding application state");
        }
        
        setIsAddingApplicationState(false);
    }
    
    if (isError) {
        return <Alert severity={"error"}>Error getting application states: {error.name}, {error.message}</Alert>;
    }
    
    if (isPending) {
        return <LinearProgress/>
    }
    
    return (
        <>
            <Helmet>
                <title>Application state - tanbyiot.app</title>
            </Helmet>
            <Breadcrumbs>
                <Typography color={"text.primary"}>Application state</Typography>
            </Breadcrumbs>

            {states && states.length === 0 && (
                <Alert severity={"warning"} style={{ marginBottom: "1em" }}>
                    No states are defined.
                </Alert>
            )}

            <Typography align={"right"} style={{ paddingBottom: "1em" }}>
                <Button variant={"contained"}
                        startIcon={<AddCircleOutlined/>}
                        onClick={() => setOpenAddApplicationStateDialog(true)}>
                    New state
                </Button>
            </Typography>
            <AddApplicationStateDialog open={openAddApplicationStateDialog}
                             onClose={() => setOpenAddApplicationStateDialog(false)}
                             onSubmit={addApplicationState}/>
            {isAddingApplicationState && <CircularProgress/>}
            {addApplicationStateError && (
                <Alert severity={"error"}
                       style={{ marginBottom: "1em" }}>
                    Error adding application state: {addApplicationStateError.name}, {addApplicationStateError.message}
                </Alert>
            )}
            <EditApplicationStateDialog open={isEditingState}
                                   state={stateBeingEdited}
                                   onClose={() => setIsEditingState(false)}
                                   onSubmit={onStateUpdated}/>
            
            <Typography align={"right"}>
                <Button onClick={refresh}>Refresh</Button>
            </Typography>
            <Typography align={"right"} variant={"subtitle2"}>
                Automatically refreshes every 10 seconds
            </Typography>

            <DataGrid columns={applicationStatesTableColumns}
                      rows={states}
                      autoHeight
                      style={{
                          width: '100%',
                          marginTop: "1em"
                      }}/>
        </>
    );
}