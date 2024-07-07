import moment from "moment";
import {AddTenantStateArgs, TenantState, UpdateTenantStateArgs} from "./types.t.ts";
import {useUser} from "./UsersApi.ts";
import {getApi} from "./Api.ts";
import { useAuth0 } from "@auth0/auth0-react";

function transformTenantStateFromServer(tenantState: TenantState) : TenantState {
    return {
        ...tenantState,
        setAt: moment(tenantState.setAt)
    };
}

export function useTenantStatesApi() {
    const {
        getAccessTokenSilently
    } = useAuth0();
    const user = useUser();

    async function getAuthenticatedApi() {
        const token = await getAccessTokenSilently();
        return getApi(token);
    }
    
    return {
        getAllTenantStates: async function(): Promise<TenantState[]> {
            const api = await getAuthenticatedApi();
            const response = await api.get<TenantState[]>(`/api/tenants/${user?.currentTenant.id}/states`);
            
            return response.data.map(transformTenantStateFromServer);
        },
        updateTenantState: async function(tenantStateId: string, args: UpdateTenantStateArgs): Promise<TenantState> {
            const api = await getAuthenticatedApi();
            const response = await api.put(`/api/tenants/${user?.currentTenant.id}/states/${tenantStateId}`, args);
            
            return transformTenantStateFromServer(response.data);
        },
        deleteTenantState: async function(tenantStateId: string) {
            const api = await getAuthenticatedApi();
            await api.delete(`/api/tenants/${user?.currentTenant.id}/states/${tenantStateId}`);
        },
        addTenantState: async function(args: AddTenantStateArgs): Promise<TenantState> {
            const api = await getAuthenticatedApi();
            const response = await api.post(`/api/tenants/${user?.currentTenant.id}/states`, args);
            
            return transformTenantStateFromServer(response.data);
        }
    };
}