import { useAuth0 } from "@auth0/auth0-react";
import {getApi} from "./Api.ts";
import {SystemUser, Tenant} from "./types.t.ts";
import { useQuery } from "@tanstack/react-query";
import {QueryKeys} from "./constants.ts";

export function useUsersApi() {
    const {
        isAuthenticated,
        getAccessTokenSilently,
        user: auth0User
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
            const getCurrentUserResponse = await api.get<SystemUser>("/api/users/current-user", {
                headers: {
                    "Email": auth0User?.email
                }
            });
            
            return getCurrentUserResponse.data;
        },
        setCurrentTenant: async function(tenant: Tenant) {
            const api = await getAuthenticatedApi();
            
            await api.put(`/api/users/set-current-tenant`, {
                tenantId: tenant.id
            });
        },
        getCurrentUserPermissions: async function(): Promise<string[]> {
            if (!isAuthenticated) {
                throw "Can't get current user permissions, not authenticated";
            }
            
            const api = await getAuthenticatedApi();
            const response = await api.get<string[]>("/api/users/current-user/permissions");
            
            return response.data;
        }
    };
}

export function useUser() {
    const api = useUsersApi();
    const {
        data: user
    } = useQuery({
        queryKey: [QueryKeys.User],
        queryFn: api.getCurrentUser
    });
    
    return user;
}

export function usePermissions() {
    const api = useUsersApi();
    const {
        data: permissions
    } = useQuery({
        queryKey: [QueryKeys.UserPermissions],
        queryFn: api.getCurrentUserPermissions
    });
    
    return {
        isSystemAdmin: permissions && permissions.indexOf("admin:all") !== -1
    };
}