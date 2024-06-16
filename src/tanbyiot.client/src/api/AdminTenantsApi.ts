﻿import { useAuth0 } from "@auth0/auth0-react";
import {getApi} from "./Api.ts";

export function useAdminTenantsApi() {
    const {
        getAccessTokenSilently
    } = useAuth0();

    async function getAuthenticatedApi() {
        const token = await getAccessTokenSilently();

        return getApi(token);
    }

    return {
        getTenants: async function(): Promise<string[]> {
            const api = await getAuthenticatedApi();
            const response = await api.get<string[]>("api/admin/tenants");
            
            return response.data;
        }
    };
}