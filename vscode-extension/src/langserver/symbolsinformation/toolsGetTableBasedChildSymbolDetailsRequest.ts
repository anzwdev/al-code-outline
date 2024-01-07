import { ToolsGetTableBasedSymbolDetailsRequest } from "./toolsGetTableBasedSymbolDetailsRequest";
import { ToolsSymbolReference } from "./toolsSymbolReference";

export class ToolsGetTableBasedChildSymbolDetailsRequest extends ToolsGetTableBasedSymbolDetailsRequest {
    childSymbolName: string;

    constructor(newPath: string | undefined, 
        newSymbolReference: ToolsSymbolReference,
        newChildSymbolName: string, 
        newGetExistingFields: boolean, newGetAvailableFields: boolean) {
        
        super(newPath, newSymbolReference, newGetExistingFields, newGetAvailableFields);

        this.childSymbolName = newChildSymbolName;
    }

}