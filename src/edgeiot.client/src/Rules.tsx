import {DataGrid, GridActionsCellItem, GridColDef, GridRowParams } from "@mui/x-data-grid";
import {Rule} from "./api/types.t.ts";
import {Alert, Breadcrumbs, Button, LinearProgress, Tooltip, Typography } from "@mui/material";
import {AddCircleOutlined, Edit } from "@mui/icons-material";
import { useQuery } from "@tanstack/react-query";
import {getAllRules} from "./api/RulesApi.ts";
import { Helmet } from "react-helmet";
import { useNavigate } from "react-router-dom";

export function Rules() {
    const navigate = useNavigate();
    
    const rulesTableColumns: GridColDef<Rule>[] = [
        {
            field: "name",
            headerName: "Name",
            flex: 1,
            renderCell: (params) => (
                <>
                    <a href={`/rules/${params.row.id}`}>{params.row.name}</a>
                </>
            )
        },
        {
            field: "enabled",
            headerName: "Enabled",
            width: 100,
            renderCell: (params) => (
                <input type={"checkbox"} disabled checked={params.row.enabled}/>
            )
        },
        {
            field: "createdAt",
            headerName: "Created"
        },
        {
            field: "actions",
            headerName: "Actions",
            getActions: (params: GridRowParams<Rule>) => [
                <GridActionsCellItem icon={<Tooltip title={"Edit rule"}><Edit/></Tooltip>}
                                     label={"Edit rule"}
                                     onClick={() => navigate(`/rules/${params.row.id}`)}/>
            ]
        }
    ];
    
    const {
        isPending,
        isError,
        error,
        data: rules
    } = useQuery({
        queryKey: ["rules"],
        queryFn: getAllRules
    });
    
    return (
        <>
            <Helmet>
                <title>Rules - edgeiot</title>
            </Helmet>
            <Breadcrumbs aria-label={"Breadcrumbs"}>
                <Typography color={"text.primary"}>Rules</Typography>
            </Breadcrumbs>
            
            {isPending && <LinearProgress/>}
            
            {isError && (
                <Alert severity={"error"}
                       style={{ marginBottom: "1em" }}>
                    Error getting rules: {error.name}, {error.message}
                </Alert>
            )}

            {rules && rules.length === 0 && (
                <Alert severity={"warning"}
                       style={{ marginBottom: "1em" }}>
                    You have no rules. You should create one now!
                </Alert>
            )}
            
            <Typography align={"right"}>
                <Button variant={"contained"}
                        startIcon={<AddCircleOutlined/>}
                        onClick={() => navigate(`/rules/new`)}>
                    New rule
                </Button>
            </Typography>

            {rules && (
                <DataGrid columns={rulesTableColumns}
                          rows={rules}
                          autoHeight
                          style={{
                              width: "100%",
                              marginTop: "1em"
                          }}/>
            )}
        </>
    );
}