import { ToolsSymbolInformationRequest } from "./toolsSymbolInformationRequest";

export class ToolsGetTableBasedSymbolDetailsRequest extends ToolsSymbolInformationRequest {
    name: string;
    getExistingFields: boolean;
    getAvailableFields: boolean;

    constructor(newPath: string | undefined, newName: string, newGetExistingFields: boolean, newGetAvailableFields: boolean) {
        super(newPath);
        this.name = newName;
        this.getExistingFields = newGetExistingFields;
        this.getAvailableFields = newGetAvailableFields;
    }

}