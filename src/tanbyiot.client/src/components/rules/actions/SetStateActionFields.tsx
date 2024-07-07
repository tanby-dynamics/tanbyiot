import { FormControl, FormLabel, TextField } from "@mui/material";
import { RuleAction } from "../../../api/types.t";
import {FormRow} from "../../shared/FormRow.tsx";
import { Editor } from "@monaco-editor/react";

export type SetStateFieldsProps = {
    action: RuleAction;
    onPayloadChange: (value: string) => void
}

export function SetStateActionFields(props: SetStateFieldsProps) {
    return (
        <>
            <FormRow>
                <FormControl fullWidth>
                    <TextField name={"key"}
                               label={"Key"}
                               defaultValue={props.action.key}
                               required
                               fullWidth
                               margin={"dense"} />
                </FormControl>
            </FormRow>
            <FormRow>
                <FormControl fullWidth>
                    <FormLabel>Value</FormLabel>
                    <Editor height={"20vh"}
                            defaultLanguage={"json"}
                            defaultValue={props.action.payload ?? ""}
                            onChange={(value) => props.onPayloadChange(value ?? '')}/>
                </FormControl>
            </FormRow>
        </>
    );
}