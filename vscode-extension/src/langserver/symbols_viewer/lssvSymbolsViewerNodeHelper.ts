import { LSSyntaxNodeKindHelper } from "../common_types/lsSyntaxNodeKindHelper";
import { LSSVSymbvolsViewerNode } from "./lssvSymbolsViewerNode";

export class LSSVSymbvolsViewerNodeHelper {

    static updateIcon(node: LSSVSymbvolsViewerNode) {
        node.icon = LSSyntaxNodeKindHelper.getIconName(node.kind, node.access, node.subtype);
    
        if (node.childSymbols) {
            for (let i=0; i < node.childSymbols.length; i++) {
                this.updateIcon(node.childSymbols[i]);
            }
        }
    }

}