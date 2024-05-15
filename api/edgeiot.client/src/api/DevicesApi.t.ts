import { Moment } from "moment";

export type Device = {
    id: string;
    name: string;
    groupName: string;
    lastConnected: string | Moment | null;
}