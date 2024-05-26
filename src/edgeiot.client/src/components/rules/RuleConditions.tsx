import {Alert, Button, Tooltip, Typography } from "@mui/material";
import {RuleCondition, RuleDetail} from "../../api/types.t.ts";
import { DataGrid, GridActionsCellItem, GridColDef } from "@mui/x-data-grid";
import { AddCircleOutlined, DeleteOutlined, Edit } from "@mui/icons-material";

export type RuleConditionProps = {
    rule: RuleDetail
}

export function RuleConditions(props: RuleConditionProps) {
    const {
        rule
    } = props;
    
    const columns: GridColDef<RuleCondition>[] = [
        {
            field: "type",
            headerName: "Type",
            width: 200
        },
        {
            field: "description",
            headerName: "Description",
            flex: 1
        },
        {
            field: "actions",
            headerName: "Actions",
            type: "actions",
            getActions: (params) => [
                <GridActionsCellItem icon={<Tooltip title={"Edit condition"}><Edit/></Tooltip>}
                                     label={"Edit condition"}
                                     onClick={() => alert("edit condition")}/>,
                <GridActionsCellItem icon={<Tooltip title={"Delete condition"}><DeleteOutlined/></Tooltip>}
                                     label={"Delete condition"}
                                     onClick={() => alert("delete condition")}/>
            ]
        }
    ];
    
    return (
        <>
            <Typography variant={"h6"} style={{ marginTop: "1em" }}>Conditions</Typography>
            
            {rule.conditions.length == 0 && (
                <Alert severity={"warning"}
                       style={{ marginBottom: "1em" }}>
                    This rule has no conditions. It will execute on all telemetries received. If this is not what you
                    intended, add a rule to limit when this rule is executed.
                </Alert>
            )}
            
            <Typography align={"right"}>
                <Button variant={"contained"}
                        startIcon={<AddCircleOutlined/>}
                        onClick={() => alert('woo')}>
                    New condition
                </Button>
            </Typography>
            
            <DataGrid columns={columns}
                      rows={rule.conditions}
                      autoHeight
                      style={{
                          width: "100%",
                          marginTop: "1em"
                      }}/>
        </>
    );
}