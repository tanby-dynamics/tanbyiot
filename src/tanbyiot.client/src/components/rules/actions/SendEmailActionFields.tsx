import { FormControl, FormLabel, TextField } from "@mui/material";
import {RuleAction} from "../../../api/types.t.ts";
import {FormRow} from "../../shared/FormRow.tsx";
import { Editor } from "@monaco-editor/react";

export type SendEmailActionFieldsProps = {
    action: RuleAction,
    onSendEmailBodyChange: (value: string) => void
}

export function SendEmailActionFields(props: SendEmailActionFieldsProps) {
    return (<>
        <FormRow>
            <FormControl fullWidth>
                <TextField name={"sendEmailSenderEmail"}
                           label={"Sender email"}
                           required
                           fullWidth
                           defaultValue={props.action.sendEmailSenderEmail}/>
            </FormControl>
        </FormRow>
        <FormRow>
            <FormControl fullWidth>
                <TextField name={"sendEmailSenderName"}
                           label={"Sender name"}
                           fullWidth
                           defaultValue={props.action.sendEmailSenderName}/>
            </FormControl>
        </FormRow>
        <FormRow>
            <FormControl fullWidth>
                <TextField name={"sendEmailToEmail"}
                           label={"Recipient"}
                           fullWidth
                           defaultValue={props.action.sendEmailToEmail}
                           helperText={<>Separate multiple recipients with a comma or semicolon (e.g. <code>rebecca@tanbyiot.app; support@tanbyiot.app</code>)</>}
                />
            </FormControl>
        </FormRow>
        <FormRow>
            <FormControl fullWidth>
                <TextField name={"sendEmailSubject"}
                           label={"Subject"}
                           fullWidth
                           defaultValue={props.action.sendEmailSubject}/>
            </FormControl>
        </FormRow>
        <FormRow>
            <FormControl fullWidth>
                <FormLabel>Body</FormLabel>
                <Editor height={"20vh"}
                        defaultLanguage={"json"}
                        defaultValue={props.action.sendEmailBody ?? ""}
                        onChange={(value) => props.onSendEmailBodyChange(value ?? "")}/>
            </FormControl>
        </FormRow>
    </>);
}