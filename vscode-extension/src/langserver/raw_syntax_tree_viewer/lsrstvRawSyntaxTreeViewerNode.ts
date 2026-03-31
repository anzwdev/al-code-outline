import { LSTextRange } from "../common_types/lsTextRange";

export interface LSRSTVRawSyntaxTreeViewerNode {
    uid: string;
    idx: number;
    name?: string;
    fullName?: string;
    containsDiagnostics: boolean;
    range?: LSTextRange;
    selectionRange?: LSTextRange;
    type?: string;
    childSymbols?: LSRSTVRawSyntaxTreeViewerNode[];
}