import axios from "axios";
import {User} from "./types.t.ts";

export function getApi(token?: string | null) {
    return axios.create({
        baseURL: import.meta.env.VITE_API_BASE_URL,
        headers: {
            ...(token ? { "Authorization": `Bearer ${token}` } : undefined)
        }
    });
}

export async function getVersion(): Promise<string> {
    const api = getApi();
    const response = await api.get<string>("/api/version");
    
    return response.data;
}