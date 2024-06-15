import { useAuth0 } from "@auth0/auth0-react";
import {getApi} from "./Api.ts";
import {SystemUser, Tenant} from "./types.t.ts";
import { useQuery } from "@tanstack/react-query";

export function useUsersApi() {
    const {
        isAuthenticated,
        getAccessTokenSilently
    } = useAuth0();

    async function getAuthenticatedApi() {
        const token = await getAccessTokenSilently();
        return getApi(token);
    }

    return {
        ready: isAuthenticated,
        getCurrentUser: async function(): Promise<SystemUser> {
            if (!isAuthenticated) {
                throw "Can't get current user, not authenticated";
            }
            
            const api = await getAuthenticatedApi();
            const getCurrentUserResponse = await api.get<SystemUser>("/api/users/current-user");
            
            return getCurrentUserResponse.data;
        },
        setCurrentTenant: async function(tenant: Tenant) {
            const api = await getAuthenticatedApi();
            
            await api.put(`/api/users/set-current-tenant`, {
                tenantId: tenant.id
            });
        }
    };
}

export function useUser() {
    const api = useUsersApi();
    const {
        data: user
    } = useQuery({
        queryKey: ["user"],
        queryFn: api.getCurrentUser
    });
    
    return user;
}