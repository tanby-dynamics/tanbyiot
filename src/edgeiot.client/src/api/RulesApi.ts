import {
    Rule,
    RuleAction,
    RuleActionType,
    RuleCondition,
    RuleConditionType,
    RuleDetail, UpdateRuleActionArgs, UpdateRuleArgs,
    UpdateRuleConditionArgs
} from "./types.t.ts";
import {useApi} from "./Api.ts";
import moment from "moment";

function transformRuleFromServer(rule: Rule): Rule {
    return {
        ...rule,
        createdAt: moment(rule.createdAt),
        updatedAt: moment(rule.updatedAt)
    };
}

function transformRuleConditionFromServer(condition: RuleCondition): RuleCondition {
    return {
        ...condition,
        createdAt: moment(condition.createdAt),
        updatedAt: moment(condition.updatedAt)
    };
}

function transformRuleActionFromServer(action: RuleAction): RuleAction {
    return {
        ...action,
        createdAt: moment(action.createdAt),
        updatedAt: moment(action.updatedAt)
    };
}

export async function getAllRules(): Promise<Rule[]> {
    const api = useApi();
    const response = await api.get<Rule[]>("api/rules");
    
    return response.data.map(transformRuleFromServer);
}

export async function addRule(name: string): Promise<Rule> {
    const api = useApi();
    const response = await api.post<Rule>(`/api/rules`, {
        name
    });
    
    return transformRuleFromServer(response.data);
}

export async function getRule(id: string): Promise<RuleDetail> {
    const api = useApi();
    const response = await api.get<RuleDetail>(`/api/rules/${id}`);
    
    return {
        ...response.data,
        createdAt: moment(response.data.createdAt),
        conditions: response.data.conditions.map(transformRuleConditionFromServer),
        actions: response.data.actions.map(transformRuleActionFromServer)
    };
}

export async function updateRule(id: string, args: UpdateRuleArgs): Promise<Rule> {
    const api = useApi();
    const response = await api.put<Rule>(`/api/rules/${id}`, args);
    
    return transformRuleFromServer(response.data);
}

export async function addRuleCondition(ruleId: string, type: RuleConditionType): Promise<RuleCondition> {
    const api = useApi();
    const response = await api.post<RuleCondition>(`/api/rules/${ruleId}/conditions`, {
        type
    });

    return transformRuleConditionFromServer(response.data);
}

export async function updateRuleCondition(ruleId: string, ruleConditionId: string, args: UpdateRuleConditionArgs): Promise<RuleCondition> {
    const api = useApi();
    const response = await api.put(`/api/rules/${ruleId}/conditions/${ruleConditionId}`, args);
    
    return transformRuleConditionFromServer(response.data);
}

export async function deleteRuleCondition(ruleId: string, ruleConditionId: string) {
    const api = useApi();
    await api.delete(`/api/rules/${ruleId}/conditions/${ruleConditionId}`);
}

export async function addRuleAction(ruleId: string, type: RuleActionType): Promise<RuleAction> {
    const api = useApi();
    const response = await api.post<RuleAction>(`/api/rules/${ruleId}/actions`, {
        type
    });

    return transformRuleActionFromServer(response.data);
}

export async function updateRuleAction(ruleId: string, ruleActionId: string, args: UpdateRuleActionArgs): Promise<RuleAction> {
    console.log(args);
    const api = useApi();
    const response = await api.put(`/api/rules/${ruleId}/actions/${ruleActionId}`, args);

    return transformRuleActionFromServer(response.data);
}

export async function deleteRuleAction(ruleId: string, ruleActionId: string) {
    const api = useApi();
    await api.delete(`/api/rules/${ruleId}/actions/${ruleActionId}`);
}

export const rulesApi = {
    getAllRules,
    addRule,
    getRule,
    addRuleCondition,
    updateRuleCondition,
    deleteRuleCondition,
    addRuleAction,
    updateRuleAction,
    deleteRuleAction,
    updateRule
}