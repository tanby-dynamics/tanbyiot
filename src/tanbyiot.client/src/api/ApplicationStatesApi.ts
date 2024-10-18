import moment from "moment";
import {AddApplicationStateArgs, ApplicationState, UpdateApplicationStateArgs} from "./types.t.ts";
import {useUser} from "./UsersApi.ts";
import {getApi} from "./Api.ts";

function transformApplicationStateFromServer(applicationState: ApplicationState) : ApplicationState {
    return {
        ...applicationState,
        setAt: moment(applicationState.setAt)
    };
}

export function useApplicationStatesApi() {
    return {
        getAllApplicationStates: async function(): Promise<ApplicationState[]> {
            const api = getApi();
            const response = await api.get<ApplicationState[]>(`/api/states`);
            
            return response.data.map(transformApplicationStateFromServer);
        },
        updateApplicationState: async function(applicationStateId: string, args: UpdateApplicationStateArgs): Promise<ApplicationState> {
            const api = getApi();
            const response = await api.put(`/api/states/${applicationStateId}`, args);
            
            return transformApplicationStateFromServer(response.data);
        },
        deleteApplicationState: async function(applicationStateId: string) {
            const api = getApi();
            await api.delete(`/api/states/${applicationStateId}`);
        },
        addApplicationState: async function(args: AddApplicationStateArgs): Promise<ApplicationState> {
            const api = getApi();
            const response = await api.post(`/api/states`, args);
            
            return transformApplicationStateFromServer(response.data);
        }
    };
}