import { LSPIObjectIdentifier } from "./lspiObjectIdentifier";
import { LSPIObjectListItem } from "./lspiObjectListItem";

export class LSPIObjectListItemHelper {

    public static toObjectIdentifierOrUndefined(item: LSPIObjectListItem | undefined) : LSPIObjectIdentifier | undefined {
        if (item) {
            return LSPIObjectListItemHelper.toObjectIdentifier(item);
        }
        return undefined;
    }

    public static toObjectIdentifier(item: LSPIObjectListItem) : LSPIObjectIdentifier {
        return {
            kind: item.kind,
            id: item.id,
            name: item.name ?? "",
            namespace: item.namespace
        };
    }

    public static toObjectIdentifierList(items: LSPIObjectListItem[] | undefined): LSPIObjectIdentifier[] | undefined {
        if (items) {
            let list: LSPIObjectIdentifier[] = [];
            for (let i=0; i<items.length; i++) {
                list.push(this.toObjectIdentifier(items[i]));
            }
            return list;
        }
        return undefined;
    }

    public static addToObjectIdentifierList(items: LSPIObjectListItem[] | undefined, list: LSPIObjectIdentifier[]) {
        if (items) {
            for (let i=0; i<items.length; i++) {
                list.push(this.toObjectIdentifier(items[i]));
            }
        }
    }

}