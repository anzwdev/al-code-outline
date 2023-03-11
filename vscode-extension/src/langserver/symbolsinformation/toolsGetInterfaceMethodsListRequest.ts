import { ToolsSymbolInformationRequest } from "./toolsSymbolInformationRequest";

export class toolsGetInterfaceMethodsListRequest extends ToolsSymbolInformationRequest {
    name: string;

    constructor(newPath: string | undefined, newName: string) {
        super(newPath, true);
        this.name = newName;
    }
}