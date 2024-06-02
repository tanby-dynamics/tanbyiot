﻿import { Helmet } from "react-helmet";
import {getRule, rulesApi} from "./api/RulesApi.ts";
import { useQuery, useQueryClient } from "@tanstack/react-query";
import { useParams } from "react-router-dom";
import { Alert, Breadcrumbs, LinearProgress, Link, Paper, Switch, Table, TableBody,
    TableCell, TableContainer, TableRow, Typography } from "@mui/material";
import {formatTimestamp} from "./helpers/formatting.ts";
import {RuleConditions} from "./components/rules/RuleConditions.tsx";
import {RuleActions} from "./components/rules/RuleActions.tsx";
import { useState } from "react";
import {EditRuleDialog} from "./components/rules/EditRuleDialog.tsx";
import { toast } from "react-toastify";

export function RuleDetails() {
    const {
        id: ruleId
    } = useParams<{ id: string}>();
    const queryClient = useQueryClient();
    const [ isEditing, setIsEditing ] = useState(false);
    
    if (ruleId === undefined) {
        return <Alert severity={"error"}>No rule ID provided in path</Alert>;
    }
    
    const {
        isPending,
        error,
        data: rule
    } = useQuery({
        queryKey: ["rule"],
        queryFn: () => getRule(ruleId)
    });
    
    async function refresh() {
        await queryClient.invalidateQueries({
            queryKey: ["rule"]
        });
    }
    
    async function onRuleUpdated() {
        await refresh();
    }
    
    async function updateRuleEnabled(enabled: boolean) {
        try {
            await rulesApi.updateRule(rule!.id, {
                name: rule!.name,
                enabled
            });
            toast.success(`${enabled ? "Enabled": "Disabled"} rule`);
            await refresh();
        } catch (error) {
            console.error("Error saving rule", error);
            toast.error("Error saving rule");
        }
    }
    
    return (
        <>
            <Helmet>
                {!rule && <title>edgeiot</title>}
                {rule && <title>Editing "{rule.name}" rule - edgeiot</title>}
            </Helmet>

            {isPending && <LinearProgress/>}
            {error && (
                <Alert severity={"error"}>Error getting rule details: {error.name}, {error.message}</Alert>
            )}
            {rule && (
                <>
                    <Breadcrumbs aria-label={"Breadcrumbs"}>
                        <Link underline={"hover"} color={"inherit"} href={"/rules"}>Rules</Link>
                        <Typography color={"text.primary"}>
                            {rule.name} {!rule.enabled && <strong>(disabled)</strong>}
                            {" "}(<Link component={"button"} onClick={() => setIsEditing(true)}>edit</Link>)
                        </Typography>
                    </Breadcrumbs>
                    
                    <TableContainer sx={{ width: 500, marginTop: "1em" }} component={Paper}>
                        <Table size={"small"} aria-label={"Rule details"}>
                            <TableBody>
                                <TableRow>
                                    <TableCell>Enabled?</TableCell>
                                    <TableCell>
                                        <Switch defaultChecked={rule.enabled}
                                                onChange={(e) => updateRuleEnabled(e.target.checked)}
                                        size={"small"}/>
                                    </TableCell>
                                </TableRow>
                                <TableRow>
                                    <TableCell>Created at</TableCell>
                                    <TableCell>{formatTimestamp(rule.createdAt)}</TableCell>
                                </TableRow>
                                <TableRow>
                                    <TableCell>Updated at</TableCell>
                                    <TableCell>{formatTimestamp(rule.updatedAt)}</TableCell>
                                </TableRow>
                            </TableBody>
                        </Table>
                    </TableContainer>
                    
                    <EditRuleDialog open={isEditing}
                                    rule={rule}
                                    onClose={() => setIsEditing(false)}
                                    onSubmit={onRuleUpdated}/>

                    <RuleConditions rule={rule} onConditionChanged={refresh}/>
                    <RuleActions rule={rule} onActionChanged={refresh}/>
                </>
            )}
        </>
    );
}