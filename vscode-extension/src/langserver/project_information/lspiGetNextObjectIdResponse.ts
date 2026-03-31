import { LSObjectKind } from "../common_types/lsObjectKind";

export interface LSPIGetNextObjectIdResponse {
    kind: LSObjectKind;
    id: number;
}