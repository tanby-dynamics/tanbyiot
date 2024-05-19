import {Device, Instruction, Telemetry} from "./types.t.ts";
import moment from "moment";
import {useApi} from "./Api.ts";

function transformDeviceFromServer(device: Device): Device {
    return {
        ...device,
        lastConnected: device.lastConnected ? moment(device.lastConnected) : null
    };
}

export async function getAllDevices(): Promise<Device[]> {
    const api = useApi();
    const response = await api.get<Device[]>("api/devices");
    
    return response.data.map(transformDeviceFromServer);
}

export async function getDevice(deviceId: string) {
    const api = useApi();
    const response = await api.get<Device>(`/api/devices/${deviceId}`);
    
    return transformDeviceFromServer(response.data);
}

export async function addDevice(name: string, groupName: string): Promise<Device> {
    const api = useApi();
    const response = await api.post<Device>("/api/devices", {
        name,
        groupName
    });
    
    return transformDeviceFromServer(response.data);
}

export async function getDeviceTelemetries(deviceId: string) {
    const api= useApi();
    const response = await api.get<Telemetry[]>(`/api/devices/telemetry/${deviceId}`);

    return response.data.map<Telemetry>(x => ({
        ...x,
        receivedAt: moment(x.receivedAt)
    }));
}
export async function getDeviceInstructions(deviceId: string) {
    const api= useApi();
    const response = await api.get<Instruction[]>(`/api/devices/instructions/${deviceId}`);

    return response.data.map<Instruction>(x => ({
        ...x,
        createdAt: moment(x.createdAt),
        sentAt: x.sentAt ? moment(x.sentAt) : null
    }));
}

export const devicesApi = {
    getAllDevices,
    getDevice,
    addDevice,
    getDeviceTelemetries,
    getDeviceInstructions
}