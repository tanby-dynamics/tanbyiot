import axios from "axios";

export function useApi() {
    // TODO get from configuration, add bearer token, etc
    return axios.create({
        baseURL: "https://localhost:7061/"
    });
}