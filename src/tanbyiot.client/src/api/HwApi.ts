import {getApi} from "./Api.ts";

export type AddTelemetryArgs = {
    deviceId: string;
    type: string;
    value: string | undefined;
    payload: string | undefined;
}

export function useHwApi() {
    return {
        addTelemetry: async function(args: AddTelemetryArgs) {
            const api = getApi();
            
            await api.post("/api/hw/add-telemetry", args);
        }
    };
}