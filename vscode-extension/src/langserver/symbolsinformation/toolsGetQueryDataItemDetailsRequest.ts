import { ToolsGetTableBasedChildSymbolDetailsRequest } from "./toolsGetTableBasedChildSymbolDetailsRequest";
import { ToolsSymbolReference } from "./toolsSymbolReference";

export class ToolsGetQueryDataItemDetailsRequest extends ToolsGetTableBasedChildSymbolDetailsRequest {
    constructor(newPath: string | undefined, newSymbolReference: ToolsSymbolReference, newChildSymbolName: string, newGetExistingFields: boolean, newGetAvailableFields: boolean) {
        super(newPath, newSymbolReference, newChildSymbolName, newGetExistingFields, newGetAvailableFields);
    }

}