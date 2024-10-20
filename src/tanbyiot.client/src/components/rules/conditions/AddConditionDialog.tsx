import {
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogContentText,
    DialogTitle,
    FormControl,
    FormControlLabel,
    FormLabel,
    Radio,
    RadioGroup,
    Tooltip
} from "@mui/material";
import {RuleConditionType} from "../../../api/enums.ts";
import {getLabelForRuleConditionType} from "../../../helpers/getEnumLabel.ts";
import { InfoOutlined } from "@mui/icons-material";

export type AddConditionDialogProps = {
    open: boolean,
    onClose: () => void,
    onSubmit: (type: RuleConditionType) => void;
}

export function AddConditionDialog(props: AddConditionDialogProps) {
    function onSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        
        const formData = new FormData(event.currentTarget);
        const formJson = Object.fromEntries((formData as any).entries());

        props.onSubmit(formJson.type);
        props.onClose();
    }
    return (
        <Dialog open={props.open}
                onClose={props.onClose}
                PaperProps={{
                    component: 'form',
                    onSubmit
                }}>
            <DialogTitle>Add rule condition</DialogTitle>
            <DialogContent>
                <DialogContentText>
                    Select the type of condition to add to the rule.
                </DialogContentText>
                <FormControl>
                    <FormLabel id={"typeLabel"}></FormLabel>
                    <RadioGroup aria-labelledby={"typeLabel"}
                                defaultValue={RuleConditionType.Telemetry}
                                name={"type"}>
                        <FormControlLabel value={RuleConditionType.Telemetry}
                                          control={<Radio/>}
                                          label={
                                              <>
                                                  {getLabelForRuleConditionType(RuleConditionType.Telemetry)}
                                                  {" "}
                                                  <Tooltip title={"Passes when telemetry type matches the specified condition"}>
                                                      <InfoOutlined fontSize={"small"}/>
                                                  </Tooltip>
                                              </>
                                          }/>
                        <FormControlLabel value={RuleConditionType.State}
                                          control={<Radio/>}
                                          label={
                                              <>
                                                  {getLabelForRuleConditionType(RuleConditionType.State)}
                                                  {" "}
                                                  <Tooltip title={"Passes when the application state matches the condition"}>
                                                      <InfoOutlined fontSize={"small"}/>
                                                  </Tooltip>
                                              </>
                                          }/>
                    </RadioGroup>
                </FormControl>
            </DialogContent>
            <DialogActions>
                <Button onClick={props.onClose}>Cancel</Button>
                <Button variant={"contained"} type={"submit"}>Add condition</Button>
            </DialogActions>
        </Dialog>
    );
}