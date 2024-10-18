import axios from "axios";

export function getApi() {
    return axios.create({
        baseURL: import.meta.env.VITE_API_BASE_URL,
        headers: {
        }
    });
}

export async function getVersion(): Promise<string> {
    const api = getApi();
    const response = await api.get<string>("/api/version");

    return response.data;
}