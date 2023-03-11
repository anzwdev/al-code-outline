export class ToolsGetPageFieldAvailableToolTipsRequest {
    path: string;
    objectType: string;
    objectName: string;
    sourceTable: string;
    fieldExpression: string;

    constructor(newPath: string, newObjectType: string, newObjectName: string, newSourceTable: string, newFieldExpression: string) {
        this.path = newPath;
        this.objectType = newObjectType;
        this.objectName = newObjectName;
        this.sourceTable = newSourceTable;
        this.fieldExpression = newFieldExpression;
    }

}