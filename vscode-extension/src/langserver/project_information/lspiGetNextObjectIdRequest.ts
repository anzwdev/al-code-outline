import { LSObjectKind } from "../common_types/lsObjectKind";

export interface LSPIGetNextObjectIdRequest {
    path: string;
    kind: LSObjectKind;
}