import {Rule} from "./types.t.ts";
import {useApi} from "./Api.ts";
import moment from "moment";

function transformRuleFromServer(rule: Rule): Rule {
    return {
        ...rule,
        createdAt: moment(rule.createdAt)
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

export const rulesApi = {
    getAllRules,
    addRule
}