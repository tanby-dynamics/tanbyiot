import { AxiosResponse } from "axios";
import api from "./api";

const addDeviceService = {
    getNewApiKey: (): Promise<AxiosResponse<string>> => {
        return api.get<string>('api/devices/get-new-api-key');
    }
};

export default addDeviceService;

