import { LSObjectKind } from "../../common_types/lsObjectKind";

export interface LSPIObjectListItem {
    kind: LSObjectKind;
    id: number;
    name?: string;
    namespace?: string;
    caption?: string;

    uid: number;
}
