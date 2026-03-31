import { LSSyntaxNodeAccessModifier } from "../common_types/lsSyntaxNodeAccessModifier";
import { LSSyntaxNodeKind } from "../common_types/lsSyntaxNodeKind";

export interface LSSVSymbvolsViewerNode {
    uid: number;
    id: number;
    namespaceName?: string;
    usings?: string[];
    name?: string;
    fullName?: string;
    kind: LSSyntaxNodeKind;
    subtype?: string;
    access: LSSyntaxNodeAccessModifier;
    extends?: string;
    source?: string;
    childSymbols?: LSSVSymbvolsViewerNode[];

    //not serialized properties populated on the client side
    library?: string;
    icon?: string;
}
