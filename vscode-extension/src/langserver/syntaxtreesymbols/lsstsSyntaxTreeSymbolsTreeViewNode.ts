import { LSSyntaxNodeAccessModifier } from "../common_types/lsSyntaxNodeAccessModifier";
import { LSSyntaxNodeKind } from "../common_types/lsSyntaxNodeKind";
import { LSTextRange } from "../common_types/lsTextRange";

export interface LSSTSyntaxTreeSymbolsTreeViewNode {
    uid: string;
    idx: number;
    id: number;
    namespaceName?: string;
    usings?: string[];
    name: string;
    fullName?: string;
    kind: LSSyntaxNodeKind;
    access: LSSyntaxNodeAccessModifier;
    subtype?: string;
    elementSubtype?: string;
    format?: string;
    extends?: string;
    source?: string;
    childSymbols?: LSSTSyntaxTreeSymbolsTreeViewNode[];
    range?: LSTextRange;
    selectionRange?: LSTextRange;
    contentRange?: LSTextRange;
    tokensRange?: LSTextRange;
    containsDiagnostics?: boolean;

}