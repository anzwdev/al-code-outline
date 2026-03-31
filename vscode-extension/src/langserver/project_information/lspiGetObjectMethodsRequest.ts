import { LSPIObjectIdentifier } from "./symbols/lspiObjectIdentifier";

export interface LSPIGetObjectMethodsRequest {
    path?: string;
    identifier?: LSPIObjectIdentifier;
    includePrivate?: boolean;
}