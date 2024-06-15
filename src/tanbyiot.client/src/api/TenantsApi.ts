import { useAuth0 } from "@auth0/auth0-react";
import {getApi} from "./Api.ts";
import {AddTenantArgs, Tenant} from "./types.t.ts";

export function useTenantsApi() {
    const {
        getAccessTokenSilently,
        isAuthenticated
    } = useAuth0();

    async function getAuthenticatedApi() {
        const token = await getAccessTokenSilently();
        return getApi(token);
    }
    
    return {
        ready: isAuthenticated,
        addTenant: async function(args: AddTenantArgs): Promise<Tenant> {
            const api = await getAuthenticatedApi();
            const response = await api.post<Tenant>("/api/tenants", args);
            
            return response.data;
        }
    }
}