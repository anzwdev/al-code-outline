import { ToolsSymbolInformationRequest } from "./toolsSymbolInformationRequest";

export class ToolsGetTableFieldsListRequest extends ToolsSymbolInformationRequest {
    table: string | undefined;
    includeDisabled: boolean;
    inlcudeObsolete: boolean;

    constructor(newPath: string | undefined, newTable: string, newIncludeDisabled: boolean, newIncludeObsolete: boolean) {
        super(newPath);
        this.table = newTable;
        this.includeDisabled = newIncludeDisabled;
        this.inlcudeObsolete = newIncludeObsolete;
    }

}