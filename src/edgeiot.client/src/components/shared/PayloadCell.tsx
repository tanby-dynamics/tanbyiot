import {jsonTryParse} from "../../helpers/helpers.ts";
import {CopyValueButton} from "./CopyValueButton.tsx";

export function PayloadCell(props: { rawPayload: string | null }) {
    const payload = jsonTryParse(props.rawPayload);
    const payloadJson = payload ? JSON.stringify(payload, null, 2) : "";

    return (
        <>
            {props.rawPayload && (
                <>
                    <CopyValueButton value={payloadJson || props.rawPayload}/>
                    <code>
                        {`${props.rawPayload.substring(0, 50)}${props.rawPayload.length > 50 ? "..." : ""}`}
                    </code>
                </>
            )}
            {!props.rawPayload && <code>---</code>}
        </>
    );
}