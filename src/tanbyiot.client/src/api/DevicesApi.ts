import {Device, Instruction, Telemetry} from "./types.t.ts";
import moment from "moment";
import {getApi} from "./Api.ts";

function transformDeviceFromServer(device: Device): Device {
    return {
        ...device,
        lastConnected: device.lastConnected ? moment(device.lastConnected) : null
    };
}

export function useDevicesApi() {
    return {
        getAllDevices: async function(): Promise<Device[]> {
            const api = getApi();
            const response = await api.get<Device[]>(`/api/devices`);

            return response.data.map(transformDeviceFromServer);
        },
        getDevice: async function(deviceId: string) {
            const api = getApi();
            const response = await api.get<Device>(`/api/devices/${deviceId}`);

            return transformDeviceFromServer(response.data);
        },
        addDevice: async function(name: string, groupName: string): Promise<Device> {
            const api = getApi();
            const response = await api.post<Device>(`/api/devices`, {
                name,
                groupName
            });

            return transformDeviceFromServer(response.data);
        },
        getDeviceTelemetries: async function(deviceId: string) {
            const api = getApi();
            const response = await api.get<Telemetry[]>(`/api/devices/${deviceId}/telemetry`);

            return response.data.map<Telemetry>(x => ({
                ...x,
                receivedAt: moment(x.receivedAt)
            }));
        },
        getDeviceInstructions: async function(deviceId: string) {
            const api = getApi();
            const response = await api.get<Instruction[]>(`/api/devices/${deviceId}/instructions`);

            return response.data.map<Instruction>(x => ({
                ...x,
                createdAt: moment(x.createdAt),
                sentAt: x.sentAt ? moment(x.sentAt) : null
            }));
        }
    }
}
