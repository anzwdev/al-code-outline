import { ToolsSymbolInformationRequest } from "./toolsSymbolInformationRequest";

export class ToolsGetPageDetailsRequest  extends ToolsSymbolInformationRequest {
    name: string;
    getPageFields: boolean;
    getAvailableFields: boolean;

    constructor(newPath: string | undefined, newName: string, newGetPageFields: boolean, newGetAvailableFields: boolean) {
        super(newPath);
        this.name = newName;
        this.getPageFields = newGetPageFields;
        this.getAvailableFields = newGetAvailableFields;
    }
}