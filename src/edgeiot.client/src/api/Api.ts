import axios from "axios";

export function useApi() {
    // TODO get from configuration, add bearer token, etc
    return axios.create({
        baseURL: "https://localhost:7061/"
    });
}

export async function getVersion(): Promise<string> {
    const api = useApi();
    const response = await api.get<string>("/api/version");
    
    return response.data;
}