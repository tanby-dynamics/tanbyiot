import { Moment } from "moment";

export type Device = {
    id: string;
    name: string;
    groupName: string;
    lastConnected: string | Moment | null;
}

export type Telemetry = {
    id: string;
    deviceId: string;
    type: string;
    value: string;
    payload: string;
    receivedAt: string | Moment;
}

export type Instruction = {
    id: string;
    deviceId: string;
    type: string;
    value: string;
    payload: string;
    createdAt: string | Moment;
    sentAt: string | Moment | null;
}