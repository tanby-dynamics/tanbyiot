import {Device, Instruction, Telemetry} from "./types.t.ts";
import moment from "moment";
import {getApi} from "./Api.ts";
import { useAuth0 } from "@auth0/auth0-react";
import { useQuery } from "@tanstack/react-query";
import {useUsersApi} from "./UsersApi.ts";

function transformDeviceFromServer(device: Device): Device {
    return {
        ...device,
        lastConnected: device.lastConnected ? moment(device.lastConnected) : null
    };
}

export function useDevicesApi() {
    const userApi = useUsersApi();
    const {
        getAccessTokenSilently,
        isAuthenticated
    } = useAuth0();
    const {
        data: user
    } = useQuery({
        queryKey: ["user"],
        queryFn: userApi.getCurrentUser
    })
    
    async function getAuthenticatedApi() {
        const token = await getAccessTokenSilently();
        return getApi(token);
    }
    
    return {
        ready: isAuthenticated,
        getAllDevices: async function(): Promise<Device[]> {
            const api = await getAuthenticatedApi();
            const response = await api.get<Device[]>(`/api/tenants/${user?.currentTenant.id}/devices`);

            return response.data.map(transformDeviceFromServer);
        },
        getDevice: async function(deviceId: string) {
            const api = await getAuthenticatedApi();
            const response = await api.get<Device>(`/api/tenants/${user?.currentTenant.id}/devices/${deviceId}`);

            return transformDeviceFromServer(response.data);
        },
        addDevice: async function(name: string, groupName: string): Promise<Device> {
            const api = await getAuthenticatedApi();
            const response = await api.post<Device>(`/api/tenants/${user?.currentTenant.id}/devices`, {
                name,
                groupName
            });

            return transformDeviceFromServer(response.data);
        },
        getDeviceTelemetries: async function(deviceId: string) {
            const api= await getAuthenticatedApi();
            const response = await api.get<Telemetry[]>(`/api/tenants/${user?.currentTenant.id}/devices/${deviceId}/telemetry`);

            return response.data.map<Telemetry>(x => ({
                ...x,
                receivedAt: moment(x.receivedAt)
            }));
        },
        getDeviceInstructions: async function(deviceId: string) {
            const api= await getAuthenticatedApi();
            const response = await api.get<Instruction[]>(`/api/tenants/${user?.currentTenant.id}/devices/${deviceId}/instructions`);

            return response.data.map<Instruction>(x => ({
                ...x,
                createdAt: moment(x.createdAt),
                sentAt: x.sentAt ? moment(x.sentAt) : null
            }));
        }
    }
}
