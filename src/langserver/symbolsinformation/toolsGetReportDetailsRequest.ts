import { ToolsGetTableBasedSymbolDetailsRequest } from "./toolsGetTableBasedSymbolDetailsRequest";

export class ToolsGetReportDetailsRequest extends ToolsGetTableBasedSymbolDetailsRequest {
    constructor(newPath: string | undefined, newName: string, newGetExistingFields: boolean, newGetAvailableFields: boolean) {
        super(newPath, newName, newGetExistingFields, newGetAvailableFields);
    }

}