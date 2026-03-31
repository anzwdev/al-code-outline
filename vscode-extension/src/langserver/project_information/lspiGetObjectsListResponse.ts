import { LSObjectKind } from "../common_types/lsObjectKind";
import { LSPIObjectListItem } from "./symbols/lspiObjectListItem";

export interface LSPIGetObjectsListResponse {
    objects?: LSPIObjectListItem[];
}