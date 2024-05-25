import {Rule} from "./types.t.ts";
import {useApi} from "./Api.ts";
import moment from "moment";

export async function getAllRules(): Promise<Rule[]> {
    const api = useApi();
    const response = await api.get<Rule[]>("api/rules");
    
    return response.data.map(rule => ({
        ...rule,
        createdAt: moment(rule.createdAt)
    }));
}