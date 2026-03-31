import { LSTableFieldClass } from "../common_types/lsTableFieldClass";
import { LSPIObjectIdentifier } from "./symbols/lspiObjectIdentifier";

export interface LSPIGetTableFieldsRequest {
    path: string;
    tableIdentifier: LSPIObjectIdentifier;
    fieldClassFilter?: LSTableFieldClass[];
    includeToolTips: boolean;
    toolTipsSourceDependencies?: string[]
}
