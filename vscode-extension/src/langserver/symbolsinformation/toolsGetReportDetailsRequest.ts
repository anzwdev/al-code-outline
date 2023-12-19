import { ToolsGetTableBasedSymbolDetailsRequest } from "./toolsGetTableBasedSymbolDetailsRequest";
import { ToolsSymbolReference } from "./toolsSymbolReference";

export class ToolsGetReportDetailsRequest extends ToolsGetTableBasedSymbolDetailsRequest {
    constructor(newPath: string | undefined, newSymbolReference: ToolsSymbolReference, newGetExistingFields: boolean, newGetAvailableFields: boolean) {
        super(newPath, newSymbolReference, newGetExistingFields, newGetAvailableFields);
    }

}