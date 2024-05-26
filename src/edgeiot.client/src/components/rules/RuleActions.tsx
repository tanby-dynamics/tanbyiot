import {Alert, Button, Tooltip, Typography } from "@mui/material";
import {RuleAction, RuleDetail} from "../../api/types.t.ts";
import { DataGrid, GridActionsCellItem, GridColDef } from "@mui/x-data-grid";
import { AddCircleOutlined, DeleteOutlined, Edit } from "@mui/icons-material";

export type RuleActionProps = {
    rule: RuleDetail
}

export function RuleActions(props: RuleActionProps) {
    const {
        rule
    } = props;

    const columns: GridColDef<RuleAction>[] = [
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
                <GridActionsCellItem icon={<Tooltip title={"Edit action"}><Edit/></Tooltip>}
                                     label={"Edit action"}
                                     onClick={() => alert("edit action")}/>,
                <GridActionsCellItem icon={<Tooltip title={"Delete action"}><DeleteOutlined/></Tooltip>}
                                     label={"Delete action"}
                                     onClick={() => alert("delete action")}/>
            ]
        }
    ];

    return (
        <>
            <Typography variant={"h6"} style={{ marginTop: "1em" }}>Actions</Typography>

            {rule.conditions.length == 0 && (
                <Alert severity={"warning"}
                       style={{ marginBottom: "1em" }}>
                    This rule has no actions. Add an action that will be performed when the rule is executed.
                </Alert>
            )}

            <Typography align={"right"}>
                <Button variant={"contained"}
                        startIcon={<AddCircleOutlined/>}
                        onClick={() => alert('woo')}>
                    New action
                </Button>
            </Typography>

            <DataGrid columns={columns}
                      rows={rule.actions}
                      autoHeight
                      style={{
                          width: "100%",
                          marginTop: "1em"
                      }}/>
        </>
    );
}