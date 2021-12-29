import { ToolsGetTableBasedSymbolDetailsRequest } from "./toolsGetTableBasedSymbolDetailsRequest";

export class ToolsGetPageDetailsRequest  extends ToolsGetTableBasedSymbolDetailsRequest {
    getToolTips: boolean;


    constructor(newPath: string | undefined, newName: string, newGetExistingFields: boolean, newGetAvailableFields: boolean, newGetToolTips: boolean) {
        super(newPath, newName, newGetExistingFields, newGetAvailableFields);
        this.getToolTips = newGetToolTips;
    }
}