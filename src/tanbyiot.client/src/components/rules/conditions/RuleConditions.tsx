import {Alert, Button, CircularProgress, Tooltip, Typography } from "@mui/material";
import {RuleCondition, RuleDetail} from "../../../api/types.t.ts";
import { DataGrid, GridActionsCellItem, GridColDef } from "@mui/x-data-grid";
import { AddCircleOutlined, DeleteOutlined, Edit } from "@mui/icons-material";
import { useState } from "react";
import { AddConditionDialog } from "./AddConditionDialog.tsx";
import {EditConditionDialog} from "./EditConditionDialog.tsx";
import {useRulesApi} from "../../../api/RulesApi.ts";
import {formatRuleConditionType} from "../../../helpers/helpers.ts";
import { toast } from "react-toastify";
import {RuleConditionType} from "../../../api/enums.ts";

export type RuleConditionsProps = {
    rule: RuleDetail,
    onConditionChanged: () => void
}

export function RuleConditions(props: RuleConditionsProps) {
    const {
        rule
    } = props;
    const [ openAddConditionDialog, setOpenAddConditionDialog] = useState(false);
    const [ isAddingCondition, setIsAddingCondition ] = useState(false);
    const [ addConditionError, setAddConditionError ] = useState<Error | null>();
    const [ isEditingCondition, setIsEditingCondition ] = useState(false);
    const [ conditionBeingEdited, setConditionBeingEdited ] = useState<RuleCondition | null>(null);
    const rulesApi = useRulesApi();
    
    const columns: GridColDef<RuleCondition>[] = [
        {
            field: "type",
            headerName: "Type",
            valueGetter: (_, condition) => formatRuleConditionType(condition.type),
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
                                     onClick={() => editCondition(params.row)}/>,
                <GridActionsCellItem icon={<Tooltip title={"Delete condition"}><DeleteOutlined/></Tooltip>}
                                     label={"Delete condition"}
                                     onClick={() => deleteCondition(params.row)}/>
            ]
        }
    ];
    
    async function addCondition(type: RuleConditionType) {
        setIsAddingCondition(true);
        setAddConditionError(null);
        
        try {
            const newCondition = await rulesApi.addRuleCondition(rule.id, type);
            
            props.onConditionChanged();            
            editCondition(newCondition);
        } catch (error) {
            setAddConditionError(error as Error);
            console.error("Error adding condition", error);
            toast.error("Error adding condition");
        }
        
        setIsAddingCondition(false);
    }
    
    async function deleteCondition(condition: RuleCondition) {
        if (!confirm("Are you sure you want to delete this condition?")) {
            return;
        }
        
        try {
            await rulesApi.deleteRuleCondition(rule.id, condition.id);
            toast.success("Deleted rule condition");
            props.onConditionChanged();
        } catch (error) {
            console.error("Error deleting rule condition", error);
            toast.error("Error deleting rule condition");
        }
    }
    
    function editCondition(condition: RuleCondition) {
        setIsEditingCondition(true);
        setConditionBeingEdited(condition);
    }
    
    function onConditionUpdated() {
        setIsEditingCondition(false);
        setConditionBeingEdited(null);   
        props.onConditionChanged();
    }
    
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
            
            <Typography align={"right"} style={{ marginBottom: "1em" }}>
                <Button variant={"contained"}
                        startIcon={<AddCircleOutlined/>}
                        onClick={() => setOpenAddConditionDialog(true)}>
                    New condition
                </Button>
            </Typography>
            <AddConditionDialog open={openAddConditionDialog}
                                onClose={() => setOpenAddConditionDialog(false)}
                                onSubmit={addCondition}/>
            {isAddingCondition && <CircularProgress/>}
            {addConditionError && (
                <Alert severity={"error"}
                       style={{ marginBottom: "1em" }}>
                    Error adding condition: {addConditionError.name}, {addConditionError.message}
                </Alert>
            )}
            <EditConditionDialog open={isEditingCondition}
                                 condition={conditionBeingEdited}
                                 rule={rule}
                                 onClose={() => setIsEditingCondition(false)}
                                 onSubmit={onConditionUpdated}/>                                 
           
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