import axios from "axios";

export function useApi() {
    return axios.create({
        baseURL: import.meta.env.API_BASE_URL ?? "https://localhost:7061/"
    });
}

export async function getVersion(): Promise<string> {
    const api = useApi();
    const response = await api.get<string>("/api/version");
    
    return response.data;
}