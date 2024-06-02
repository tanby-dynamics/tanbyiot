import { Helmet } from "react-helmet";
import {getRule} from "./api/RulesApi.ts";
import { useQuery, useQueryClient } from "@tanstack/react-query";
import { useParams } from "react-router-dom";
import { Alert, Breadcrumbs, LinearProgress, Link, Paper, Table, TableBody,
    TableCell, TableContainer, TableRow, Typography } from "@mui/material";
import {formatTimestamp} from "./helpers/formatting.ts";
import {RuleConditions} from "./components/rules/RuleConditions.tsx";
import {RuleActions} from "./components/rules/RuleActions.tsx";

export function RuleDetails() {
    const {
        id: ruleId
    } = useParams<{ id: string}>();
    const queryClient = useQueryClient();
    
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
                        <Typography color={"text.primary"}>{rule.name}</Typography>
                    </Breadcrumbs>
                    
                    <TableContainer sx={{ width: 500, marginTop: "1em" }} component={Paper}>
                        <Table size={"small"} aria-label={"Rule details"}>
                            <TableBody>
                                <TableRow>
                                    <TableCell>Created at</TableCell>
                                    <TableCell>{formatTimestamp(rule.createdAt)}</TableCell>
                                </TableRow>
                                <TableRow>
                                    <TableCell>Enabled?</TableCell>
                                    <TableCell>{rule.enabled ? "Yes" : "No"}</TableCell>
                                </TableRow>
                            </TableBody>
                        </Table>
                    </TableContainer>

                    <RuleConditions rule={rule} onConditionChanged={refresh}/>
                    <RuleActions rule={rule} onActionChanged={refresh}/>
                </>
            )}
        </>
    );
}