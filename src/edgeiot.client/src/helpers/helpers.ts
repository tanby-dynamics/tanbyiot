export function jsonTryParse(json: string | null): any | null {
    if (!json) {
        return null;
    }
    try {
        return JSON.parse(json);
    } catch {
        return null;
    }
}