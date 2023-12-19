import { ToolsSymbolInformationRequest } from "./toolsSymbolInformationRequest";
import { ToolsSymbolReference } from "./toolsSymbolReference";

export class toolsGetInterfaceMethodsListRequest extends ToolsSymbolInformationRequest {
    symbolReference: ToolsSymbolReference;

    constructor(newPath: string | undefined, newSymbolReference: ToolsSymbolReference) {
        super(newPath, true);
        this.symbolReference = newSymbolReference;
    }
}