import {SystemUser} from "./types.t.ts";
import {getApi} from "./Api.ts";
import { useAuth0 } from "@auth0/auth0-react";

export function useAdminUsersApi() {
    const {
        getAccessTokenSilently
    } = useAuth0();

    async function getAuthenticatedApi() {
        const token = await getAccessTokenSilently();

        return getApi(token);
    }
    
    return {
        getUsers: async function(): Promise<SystemUser[]> {
            const api = await getAuthenticatedApi();
            const response = await api.get<SystemUser[]>("/api/admin/users");
            
            return response.data;
        }
    };
}