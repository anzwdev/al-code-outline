import { ToolsGetTableBasedSymbolDetailsRequest } from "./toolsGetTableBasedSymbolDetailsRequest";
import { ToolsSymbolReference } from "./toolsSymbolReference";

export class ToolsGetPageDetailsRequest  extends ToolsGetTableBasedSymbolDetailsRequest {
    getToolTips: boolean;
    toolTipsSourceDependencies: string[] | undefined;


    constructor(newPath: string | undefined, newSymbolReference: ToolsSymbolReference, newGetExistingFields: boolean, newGetAvailableFields: boolean, newGetToolTips: boolean, newToolTipsSourceDependencies: string[] | undefined) {
        super(newPath, newSymbolReference, newGetExistingFields, newGetAvailableFields);
        this.getToolTips = newGetToolTips;
        this.toolTipsSourceDependencies = newToolTipsSourceDependencies;
    }
}