import { ToolsSymbolInformationRequest } from "./toolsSymbolInformationRequest";

export class ToolsGetTableFieldsListRequest extends ToolsSymbolInformationRequest {
    table: string | undefined;
    includeDisabled: boolean;
    includeObsolete: boolean;
    includeNormal: boolean;
    includeFlowFields: boolean;
    includeFlowFilters: boolean;
    includeToolTips: boolean;

    constructor(newPath: string | undefined, newTable: string, newIncludeDisabled: boolean, newIncludeObsolete: boolean, newIncludeNormal: boolean, newIncludeFlowFields: boolean, newIncludeFlowFilters: boolean, newIncludeToolTips: boolean) {
        super(newPath, true);
        this.table = newTable;
        this.includeDisabled = newIncludeDisabled;
        this.includeObsolete = newIncludeObsolete;
        this.includeNormal = newIncludeNormal;
        this.includeFlowFields = newIncludeFlowFields;
        this.includeFlowFilters = newIncludeFlowFilters;
        this.includeToolTips = newIncludeToolTips;
    }

}