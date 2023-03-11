import { ToolsSymbolInformationRequest } from "./toolsSymbolInformationRequest";

export class toolsGetCodeunitMethodsListRequest extends ToolsSymbolInformationRequest {
    name: string;

    constructor(newPath: string | undefined, newName: string) {
        super(newPath, true);
        this.name = newName;
    }
}