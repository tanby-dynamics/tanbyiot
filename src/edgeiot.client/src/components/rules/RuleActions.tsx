import {Alert, Button, CircularProgress, Tooltip, Typography} from "@mui/material";
import {RuleAction, RuleActionType, RuleDetail} from "../../api/types.t.ts";
import {DataGrid, GridActionsCellItem, GridColDef} from "@mui/x-data-grid";
import {AddCircleOutlined, DeleteOutlined, Edit} from "@mui/icons-material";
import {useState} from "react";
import moment from "moment";
import {EditActionDialog} from "./EditActionDialog.tsx";
import {AddActionDialog} from "./AddActionDialog.tsx";

export type RuleActionProps = {
    rule: RuleDetail,
    onActionChanged: () => void;
}

export function RuleActions(props: RuleActionProps) {
    const {
        rule
    } = props;
    const [openAddActionDialog, setOpenAddActionDialog] = useState(false);
    const [isAddingAction, setIsAddingAction] = useState(false);
    const [addActionError, setAddActionError] = useState<Error | null>();
    const [isEditingAction, setIsEditingAction] = useState(false);
    const [actionBeingEdited, setActionBeingEdited] = useState<RuleAction | null>(null);

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
                                     onClick={() => editAction(params.row)}/>,
                <GridActionsCellItem icon={<Tooltip title={"Delete action"}><DeleteOutlined/></Tooltip>}
                                     label={"Delete action"}
                                     onClick={() => alert("delete action")}/>
            ]
        }
    ];

    async function addAction(type: RuleActionType) {
        setIsAddingAction(true);
        setAddActionError(null);

        try {
            //const response = await rulesApi.addAction(rule.id, type);
            editAction({
                id: "an id",
                createdAt: moment(),
                type
            });
        } catch (error) {
            setAddActionError(error as Error);
            console.error("Error adding action", error)
        }

        setIsAddingAction(false);
    }

    function editAction(action: RuleAction) {
        setIsEditingAction(true);
        setActionBeingEdited(action);
    }

    async function updateAction() {
        setIsEditingAction(false);
        setActionBeingEdited(null);
        // TODO update actionBeingEdited
        props.onActionChanged();
    }

    return (
        <>
            <Typography variant={"h6"} style={{marginTop: "1em"}}>Actions</Typography>

            {rule.conditions.length == 0 && (
                <Alert severity={"warning"}
                       style={{marginBottom: "1em"}}>
                    This rule has no actions. Add an action that will be performed when the rule is executed.
                </Alert>
            )}

            <Typography align={"right"} style={{marginBottom: "1em"}}>
                <Button variant={"contained"}
                        startIcon={<AddCircleOutlined/>}
                        onClick={() => setOpenAddActionDialog(true)}>
                    New action
                </Button>
            </Typography>
            <AddActionDialog open={openAddActionDialog}
                             onClose={() => setOpenAddActionDialog(false)}
                             onSubmit={addAction}/>
            {isAddingAction && <CircularProgress/>}
            {addActionError && (
                <Alert severity={"error"}
                       style={{marginBottom: "1em"}}>
                    Error adding condition: {addActionError.name}, {addActionError.message}
                </Alert>
            )}
            <EditActionDialog open={isEditingAction}
                              action={actionBeingEdited}
                              onClose={() => setIsEditingAction(false)}
                              onSubmit={updateAction}/>

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