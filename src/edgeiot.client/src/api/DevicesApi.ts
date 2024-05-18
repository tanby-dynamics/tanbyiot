import axios from "axios";
import {Device} from "./DevicesApi.t.ts";
import moment from "moment";

// TODO move into Api.ts
const api = axios.create({
    baseURL: "https://localhost:7061/"
});

function transformDeviceFromServer(device: Device): Device {
    return {
        ...device,
        lastConnected: device.lastConnected ? moment(device.lastConnected) : null
    };
}

export async function getAllDevices(): Promise<Device[]> {
    const response = await api.get<Device[]>("api/devices");
    
    return response.data.map(transformDeviceFromServer);
}

export async function addDevice(name: string, groupName: string): Promise<Device> {
    const response = await api.post<Device>("/api/devices", {
        name,
        groupName
    });
    
    return transformDeviceFromServer(response.data);
}

export const devicesApi = {
    getAllDevices,
    addDevice
}