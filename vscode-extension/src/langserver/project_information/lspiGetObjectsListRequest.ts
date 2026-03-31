import { LSObjectKind } from "../common_types/lsObjectKind";
import { LSPIGetObjectsListFilter } from "./lspiGetObjectsListFilter";

export interface LSPIGetObjectsListRequest {
    path: string;
    filter?: LSPIGetObjectsListFilter;
}