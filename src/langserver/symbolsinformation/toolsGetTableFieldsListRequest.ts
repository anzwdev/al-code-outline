import { ToolsSymbolInformationRequest } from "./toolsSymbolInformationRequest";

export class ToolsGetTableFieldsListRequest extends ToolsSymbolInformationRequest {
    table: string | undefined;

    constructor(newPath: string | undefined, newTable: string) {
        super(newPath);
        this.table = newTable;
    }

}