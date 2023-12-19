import { ToolsSymbolInformationRequest } from "./toolsSymbolInformationRequest";
import { ToolsSymbolReference } from "./toolsSymbolReference";

export class ToolsGetTableBasedSymbolDetailsRequest extends ToolsSymbolInformationRequest {
    symbolReference: ToolsSymbolReference;
    getExistingFields: boolean;
    getAvailableFields: boolean;

    constructor(newPath: string | undefined, newSymbolReference: ToolsSymbolReference, newGetExistingFields: boolean, newGetAvailableFields: boolean) {
        super(newPath, true);
        this.symbolReference = newSymbolReference;
        this.getExistingFields = newGetExistingFields;
        this.getAvailableFields = newGetAvailableFields;
    }

}