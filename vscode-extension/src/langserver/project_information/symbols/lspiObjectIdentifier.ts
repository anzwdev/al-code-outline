import { LSObjectKind } from "../../common_types/lsObjectKind";

export interface LSPIObjectIdentifier {
    kind: LSObjectKind,
    id: number;
    name: string;
    namespace?: string;
}
