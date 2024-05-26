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
import {RuleConditionType} from "../../api/types.t.ts";
import {getLabelForRuleConditionType} from "../../helpers/getEnumLabel.ts";
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
                                defaultValue={RuleConditionType.telemetryType}
                                name={"type-group"}>
                        <FormControlLabel value={RuleConditionType.telemetryType}
                                          control={<Radio/>}
                                          label={
                                              <>
                                                  {getLabelForRuleConditionType(RuleConditionType.telemetryType)}
                                                  {" "}
                                                  <Tooltip title={"Passes when the telemetry type matches the specified type"}>
                                                      <InfoOutlined fontSize={"small"}/>
                                                  </Tooltip>
                                              </>
                                          }/>
                        <FormControlLabel value={RuleConditionType.value}
                                          control={<Radio/>}
                                          label={
                                              <>
                                                  {getLabelForRuleConditionType(RuleConditionType.value)}
                                                  {" "}
                                                  <Tooltip title={"Passes based on the value of the telemetry"}>
                                                      <InfoOutlined fontSize={"small"}/>
                                                  </Tooltip>
                                              </>
                                          }/>
                        <FormControlLabel value={RuleConditionType.payload}
                                          control={<Radio/>}
                                          label={
                                              <>
                                                  {getLabelForRuleConditionType(RuleConditionType.payload)}
                                                  {" "}
                                                  <Tooltip title={"Passes when the payload of the telemetry matches the condition criteria"}>
                                                      <InfoOutlined fontSize={"small"}/>
                                                  </Tooltip>
                                              </>
                                          }/>
                        <FormControlLabel value={RuleConditionType.deviceId}
                                          control={<Radio/>}
                                          label={
                                              <>
                                                  {getLabelForRuleConditionType(RuleConditionType.deviceId)}
                                                  {" "}
                                                  <Tooltip title={"Passes when telemetry is received from the specified device"}>
                                                      <InfoOutlined fontSize={"small"}/>
                                                  </Tooltip>
                                              </>
                                          }/>
                        <FormControlLabel value={RuleConditionType.group}
                                          control={<Radio/>}
                                          label={
                                              <>
                                                  {getLabelForRuleConditionType(RuleConditionType.group)}
                                                  {" "}
                                                  <Tooltip title={"Passes when telemetry is received from a device in the specified group"}>
                                                      <InfoOutlined fontSize={"small"}/>
                                                  </Tooltip>
                                              </>
                                          }/>
                    </RadioGroup>
                </FormControl>
            </DialogContent>
            <DialogActions>
                <Button onClick={props.onClose}>Cancel</Button>
                <Button type={"submit"}>Add</Button>
            </DialogActions>
        </Dialog>
    );
}