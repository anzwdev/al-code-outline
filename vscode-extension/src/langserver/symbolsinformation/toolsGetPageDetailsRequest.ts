import { ToolsGetTableBasedSymbolDetailsRequest } from "./toolsGetTableBasedSymbolDetailsRequest";

export class ToolsGetPageDetailsRequest  extends ToolsGetTableBasedSymbolDetailsRequest {
    getToolTips: boolean;
    toolTipsSourceDependencies: string[] | undefined;


    constructor(newPath: string | undefined, newName: string, newGetExistingFields: boolean, newGetAvailableFields: boolean, newGetToolTips: boolean, newToolTipsSourceDependencies: string[] | undefined) {
        super(newPath, newName, newGetExistingFields, newGetAvailableFields);
        this.getToolTips = newGetToolTips;
        this.toolTipsSourceDependencies = newToolTipsSourceDependencies;
    }
}