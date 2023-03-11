import { ToolsGetTableBasedChildSymbolDetailsRequest } from "./toolsGetTableBasedChildSymbolDetailsRequest";

export class ToolsGetXmlPortTableElementDetailsRequest extends ToolsGetTableBasedChildSymbolDetailsRequest {
    constructor(newPath: string | undefined, newObjectName: string, newName: string, newGetExistingFields: boolean, newGetAvailableFields: boolean) {
        super(newPath, newObjectName, newName, newGetExistingFields, newGetAvailableFields);
    }
}