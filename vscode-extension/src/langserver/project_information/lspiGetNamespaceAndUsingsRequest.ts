import { LSPIObjectIdentifier } from "./symbols/lspiObjectIdentifier";

export interface LSPIGetNamespaceAndUsingsRequest {
    path?: string;
    objectIdentifier?: LSPIObjectIdentifier;
    referencedObjectsIdentifiers?: LSPIObjectIdentifier[];
    force?: boolean;
    rootNamespace?: string;
}