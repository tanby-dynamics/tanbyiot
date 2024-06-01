import {Rule, RuleAction, RuleActionType, RuleCondition, RuleConditionType, RuleDetail} from "./types.t.ts";
import {useApi} from "./Api.ts";
import moment from "moment";

function transformRuleFromServer(rule: Rule): Rule {
    return {
        ...rule,
        createdAt: moment(rule.createdAt)
    };
}

function transformRuleConditionFromServer(condition: RuleCondition): RuleCondition {
    return {
        ...condition,
        createdAt: moment(condition.createdAt)
    };
}

function transformRuleActionFromServer(action: RuleAction): RuleAction {
    return {
        ...action,
        createdAt: moment(action.createdAt)
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

export async function addCondition(ruleId: string, type: RuleConditionType): Promise<RuleCondition> {
    const api = useApi();
    const response = await api.post<RuleCondition>(`/api/rules/${ruleId}/conditions`, {
        type
    });

    return transformRuleConditionFromServer(response.data);
}

export async function addAction(ruleId: string, type: RuleActionType): Promise<RuleAction> {
    const api = useApi();
    const response = await api.post<RuleAction>(`/api/rules/${ruleId}/actions`, {
        type
    });

    return transformRuleActionFromServer(response.data);
}

export const rulesApi = {
    getAllRules,
    addRule,
    getRule,
    addCondition,
    addAction
}