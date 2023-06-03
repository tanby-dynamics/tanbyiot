import { AxiosResponse } from "axios";
import api from "./api";

export default {
    getNewApiKey: (): Promise<AxiosResponse<string>> => {
        return api.get<string>('api/devices/get-new-api-key');
    }
};