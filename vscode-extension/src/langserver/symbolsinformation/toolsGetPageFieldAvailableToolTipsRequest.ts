import { ToolsSymbolReference } from "./toolsSymbolReference";

export class ToolsGetPageFieldAvailableToolTipsRequest {
    path: string;
    objectType: string;
    objectReference: ToolsSymbolReference;
    sourceTableReference: ToolsSymbolReference;
    fieldExpression: string;

    constructor(newPath: string, newObjectType: string, newObjectReference: ToolsSymbolReference, newSourceTableReference: ToolsSymbolReference, newFieldExpression: string) {
        this.path = newPath;
        this.objectType = newObjectType;
        this.objectReference = newObjectReference;
        this.sourceTableReference = newSourceTableReference;
        this.fieldExpression = newFieldExpression;
    }

}