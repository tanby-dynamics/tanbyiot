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
import {RuleActionType} from "../../api/types.t.ts";
import {getLabelForRuleActionType} from "../../helpers/getEnumLabel.ts";
import {InfoOutlined} from "@mui/icons-material";

export type AddActionDialogProps = {
    open: boolean,
    onClose: () => void,
    onSubmit: (type: RuleActionType) => void;
}

export function AddActionDialog(props: AddActionDialogProps) {
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
            <DialogTitle>Add rule action</DialogTitle>
            <DialogContent>
                <DialogContentText>
                    Select the type of action to add to the rule.
                </DialogContentText>
                <FormControl>
                    <FormLabel id={"typeLabel"}></FormLabel>
                    <RadioGroup aria-labelledby={"typeLabel"}
                                defaultValue={RuleActionType.sendInstruction}
                                name={"type"}>
                        <FormControlLabel value={RuleActionType.sendInstruction}
                                          control={<Radio/>}
                                          label={
                                              <>
                                                  {getLabelForRuleActionType(RuleActionType.sendInstruction)}
                                                  {" "}
                                                  <Tooltip
                                                      title={"Sends an instruction to a device or group of devices"}>
                                                      <InfoOutlined fontSize={"small"}/>
                                                  </Tooltip>
                                              </>
                                          }/>
                        <FormControlLabel value={RuleActionType.triggerWebhook}
                                          control={<Radio/>}
                                          label={
                                              <>
                                                  {getLabelForRuleActionType(RuleActionType.triggerWebhook)}
                                                  {" "}
                                                  <Tooltip title={"Sends a POST request to a specified URL"}>
                                                      <InfoOutlined fontSize={"small"}/>
                                                  </Tooltip>
                                              </>
                                          }/>
                        <FormControlLabel value={RuleActionType.sendEmail}
                                          control={<Radio/>}
                                          label={
                                              <>
                                                  {getLabelForRuleActionType(RuleActionType.sendEmail)}
                                                  {" "}
                                                  <Tooltip title={"Sends an email to one or more recipients"}>
                                                      <InfoOutlined fontSize={"small"}/>
                                                  </Tooltip>
                                              </>
                                          }/>
                        <FormControlLabel value={RuleActionType.sendSMS}
                                          control={<Radio/>}
                                          label={
                                              <>
                                                  {getLabelForRuleActionType(RuleActionType.sendSMS)}
                                                  {" "}
                                                  <Tooltip title={"Sends an SMS to one or more recipients"}>
                                                      <InfoOutlined fontSize={"small"}/>
                                                  </Tooltip>
                                              </>
                                          }/>
                    </RadioGroup>
                </FormControl>
            </DialogContent>
            <DialogActions>
                <Button onClick={props.onClose}>Cancel</Button>
                <Button variant={"contained"} type={"submit"}>Add action</Button>
            </DialogActions>
        </Dialog>
    );
}