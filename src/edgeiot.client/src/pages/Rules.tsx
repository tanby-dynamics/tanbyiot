import {DataGrid, GridActionsCellItem, GridColDef } from "@mui/x-data-grid";
import {Alert, Breadcrumbs, Button, CircularProgress, LinearProgress, Tooltip, Typography } from "@mui/material";
import {AddCircleOutlined, Edit } from "@mui/icons-material";
import { useQuery } from "@tanstack/react-query";
import { Helmet } from "react-helmet";
import { useNavigate } from "react-router-dom";
import { useState } from "react";
import {Rule} from "../api/types.t.ts";
import {useRulesApi} from "../api/RulesApi.ts";
import {formatTimestamp} from "../helpers/formatting.ts";
import {AddRuleDialog} from "../components/rules/AddRuleDialog.tsx";

export function Rules() {
    const navigate = useNavigate();
    const [ openAddRuleDialog, setOpenAddRuleDialog ] = useState(false);
    const [ isAddingRule, setIsAddingRule ] = useState(false);
    const [ addRuleError, setAddRuleError] = useState<Error>();
    const rulesApi = useRulesApi();
    
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
            type: "boolean"
        },
        {
            field: "createdAt",
            headerName: "Created",
            width: 200,
            valueGetter: (_, rule) => formatTimestamp(rule.createdAt)
        },
        {
            field: "actions",
            headerName: "Actions",
            type: "actions",
            getActions: (params) => [
                <GridActionsCellItem icon={<Tooltip title={"Edit rule"}><Edit/></Tooltip>}
                                     label={"Edit rule"}
                                     onClick={() => navigate(`/rules/${params.row.id}`)}/>
            ]
        }
    ];
    
    const {
        isPending: isGetRulesPending,
        isError: isGetRulesError,
        error: getRulesError,
        data: rules
    } = useQuery({
        queryKey: ["rules"],
        queryFn: rulesApi.getAllRules
    });
    
    async function addRule(name: string) {
        setIsAddingRule(true);
        
        try {
            const response = await rulesApi.addRule(name);

            navigate(`/rules/${response.id}`);
        } catch (error) {
            setAddRuleError(error as Error);
            console.error("Error adding rule", error);
        }

        setIsAddingRule(false);
    }
    
    return (
        <>
            <Helmet>
                <title>Rules - edgeiot</title>
            </Helmet>
            <Breadcrumbs aria-label={"Breadcrumbs"}>
                <Typography color={"text.primary"}>Rules</Typography>
            </Breadcrumbs>
            
            {isGetRulesPending && <LinearProgress/>}
            
            {isGetRulesError && (
                <Alert severity={"error"}
                       style={{ marginBottom: "1em" }}>
                    Error getting rules: {getRulesError.name}, {getRulesError.message}
                </Alert>
            )}

            {rules && rules.length === 0 && (
                <Alert severity={"warning"}
                       style={{ marginBottom: "1em" }}>
                    You have no rules. You should create one now!
                </Alert>
            )}
            
            <Typography align={"right"}
                        style={{ marginBottom: "1em" }}>
                <Button variant={"contained"}
                        startIcon={<AddCircleOutlined/>}
                        onClick={() => setOpenAddRuleDialog(true)}>
                    New rule
                </Button>
            </Typography>
            <AddRuleDialog open={openAddRuleDialog}
                           onClose={() => setOpenAddRuleDialog(false)}
                           onSubmit={addRule}/>
            {isAddingRule && <CircularProgress/>}
            {addRuleError && (
                <Alert severity={"error"}
                       style={{ marginBottom: "1em" }}>
                    Error adding rule: {addRuleError.name}, {addRuleError.message}
                </Alert>
            )}

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