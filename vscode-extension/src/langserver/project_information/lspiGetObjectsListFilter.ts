import { LSObjectKind } from "../common_types/lsObjectKind";

export interface LSPIGetObjectsListFilter {
    kind: LSObjectKind;
    appIdFilter?: string[];
    skipDependencies?: boolean;
    excludeFullInherentPermissions?: boolean;
}