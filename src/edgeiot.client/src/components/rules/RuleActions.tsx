import {Alert, Button, CircularProgress, Tooltip, Typography} from "@mui/material";
import {RuleAction, RuleActionType, RuleDetail} from "../../api/types.t.ts";
import {DataGrid, GridActionsCellItem, GridColDef} from "@mui/x-data-grid";
import {AddCircleOutlined, DeleteOutlined, Edit} from "@mui/icons-material";
import {useState} from "react";
import {EditActionDialog} from "./EditActionDialog.tsx";
import {AddActionDialog} from "./AddActionDialog.tsx";
import {rulesApi} from "../../api/RulesApi.ts";
import {formatRuleActionType} from "../../helpers/helpers.ts";
import { toast } from "react-toastify";

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
            width: 200,
            valueGetter: (_, action) => formatRuleActionType(action.type)
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
                                     onClick={() => deleteAction(params.row)}/>
            ]
        }
    ];

    async function addAction(type: RuleActionType) {
        setIsAddingAction(true);
        setAddActionError(null);

        try {
            const newAction = await rulesApi.addRuleAction(rule.id, type);
            
            props.onActionChanged();            
            editAction(newAction);
        } catch (error) {
            setAddActionError(error as Error);
            console.error("Error adding action", error)
            toast.error("Error adding action");
        }

        setIsAddingAction(false);
    }
    
    async function deleteAction(action: RuleAction) {
        // TODO confirm dialog
        if (confirm("Are you sure you want to delete this action?")) {
            await rulesApi.deleteRuleAction(rule.id, action.id);
            toast.success("Deleted rule action");
            props.onActionChanged();
        }
    }

    function editAction(action: RuleAction) {
        setIsEditingAction(true);
        setActionBeingEdited(action);
    }

    function onActionUpdated() {
        setIsEditingAction(false);
        setActionBeingEdited(null);
        props.onActionChanged();
    }

    return (
        <>
            <Typography variant={"h6"} style={{marginTop: "1em"}}>Actions</Typography>

            {rule.actions.length == 0 && (
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
                              rule={rule}
                              onClose={() => setIsEditingAction(false)}
                              onSubmit={onActionUpdated}/>

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