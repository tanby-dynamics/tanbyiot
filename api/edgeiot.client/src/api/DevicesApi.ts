import axios, { AxiosResponse } from "axios";
import {Device} from "./DevicesApi.t.ts";
import moment from "moment";

// TODO move into Api.ts
const api = axios.create({
    baseURL: "https://localhost:7061/"
});

export async function getAllDevices(): Promise<Device[]> {
    const response = await api.get<Device[]>("api/devices");
    
    return response.data.map((device) => ({
        ...device,
        lastConnected: device.lastConnected ? moment(device.lastConnected) : null
    }));
}