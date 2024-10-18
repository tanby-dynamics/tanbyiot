import {
    Rule,
    RuleAction,
    RuleCondition,
    RuleDetail, UpdateRuleActionArgs, UpdateRuleArgs,
    UpdateRuleConditionArgs
} from "./types.t.ts";
import {getApi} from "./Api.ts";
import moment from "moment";
import {RuleActionType, RuleConditionType} from "./enums.ts";

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

export function useRulesApi() {
    return {
        getAllRules: async function(): Promise<Rule[]> {
            const api = getApi();
            const response = await api.get<Rule[]>(`/api/rules`);

            return response.data.map(transformRuleFromServer);
        },
        addRule: async function(name: string): Promise<Rule> {
            const api = getApi();
            const response = await api.post<Rule>(`/api/rules`, {
                name
            });

            return transformRuleFromServer(response.data);
        },
        getRule: async function(id: string): Promise<RuleDetail> {
            const api = getApi();
            const response = await api.get<RuleDetail>(`/api/rules/${id}`);

            return {
                ...response.data,
                createdAt: moment(response.data.createdAt),
                conditions: response.data.conditions.map(transformRuleConditionFromServer),
                actions: response.data.actions.map(transformRuleActionFromServer)
            };
        },
        updateRule: async function(id: string, args: UpdateRuleArgs): Promise<Rule> {
            const api = getApi();
            const response = await api.put<Rule>(`/api/rules/${id}`, args);

            return transformRuleFromServer(response.data);
        },
        addRuleCondition: async function(ruleId: string, type: RuleConditionType): Promise<RuleCondition> {
            const api = getApi();
            const response = await api.post<RuleCondition>(`/api/rules/${ruleId}/conditions`, {
                type
            });

            return transformRuleConditionFromServer(response.data);
        },
        updateRuleCondition: async function(ruleId: string, args: UpdateRuleConditionArgs): Promise<RuleCondition> {
            const api = getApi();
            const response = await api.put(`/api/rules/${ruleId}/conditions`, args);

            return transformRuleConditionFromServer(response.data);
        },
        deleteRuleCondition: async function(ruleId: string, ruleConditionId: string) {
            const api = getApi();
            await api.delete(`/api/rules/${ruleId}/conditions/${ruleConditionId}`);
        },
        addRuleAction: async function(ruleId: string, type: RuleActionType): Promise<RuleAction> {
            const api = getApi();
            const response = await api.post<RuleAction>(`/api/rules/${ruleId}/actions`, {
                type
            });

            return transformRuleActionFromServer(response.data);
        },
        updateRuleAction: async function(ruleId: string, ruleActionId: string, args: UpdateRuleActionArgs): Promise<RuleAction> {
            const api = getApi();
            const response = await api.put(`/api/rules/${ruleId}/actions/${ruleActionId}`, args);

            return transformRuleActionFromServer(response.data);
        },
        deleteRuleAction: async function(ruleId: string, ruleActionId: string) {
            const api = getApi();
            await api.delete(`/api/rules/${ruleId}/actions/${ruleActionId}`);
        }
    };
}
