﻿import moment from "moment";
import { Moment } from "moment";

export function formatTimestamp(timestamp: string | Moment | null, nullMessage: string = "---") {
    if (!timestamp) {
        return nullMessage;
    }
    if (typeof timestamp === 'string') {
        return formatTimestamp(moment(timestamp));
    }
    
    if (!timestamp.isValid()) {
        return nullMessage;
    }

    return timestamp.format();
}

export function formatRelativeTimestamp(timestamp: string | Moment | null, nullMessage: string = "---") {
    if (timestamp === null) {
        return nullMessage;
    }
    if (typeof timestamp === 'string') {
        return formatRelativeTimestamp(moment(timestamp));
    }

    return timestamp.fromNow();
}