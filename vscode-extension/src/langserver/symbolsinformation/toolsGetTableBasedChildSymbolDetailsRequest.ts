import { ToolsGetTableBasedSymbolDetailsRequest } from "./toolsGetTableBasedSymbolDetailsRequest";

export class ToolsGetTableBasedChildSymbolDetailsRequest extends ToolsGetTableBasedSymbolDetailsRequest {
    objectName: string;

    constructor(newPath: string | undefined, newObjectName: string, newName: string, newGetExistingFields: boolean, newGetAvailableFields: boolean) {
        super(newPath, newName, newGetExistingFields, newGetAvailableFields);
        this.objectName = newObjectName;
    }

}