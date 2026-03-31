import { LSPIObjectListItem } from "./symbols/lspiObjectListItem";
import { LSPITableFieldListItem } from "./symbols/lspiTableFieldListItem";

export interface LSPIGetTableFieldsResponse {
    fields?: LSPITableFieldListItem[];
}