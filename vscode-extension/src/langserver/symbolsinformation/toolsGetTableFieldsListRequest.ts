import { ToolsSymbolInformationRequest } from "./toolsSymbolInformationRequest";
import { ToolsSymbolReference } from "./toolsSymbolReference";

export class ToolsGetTableFieldsListRequest extends ToolsSymbolInformationRequest {
    tableReference: ToolsSymbolReference;
    includeDisabled: boolean;
    includeObsolete: boolean;
    includeNormal: boolean;
    includeFlowFields: boolean;
    includeFlowFilters: boolean;
    includeToolTips: boolean;
    toolTipsSourceDependencies: string[] | undefined;

    constructor(newPath: string | undefined, newTableReference: ToolsSymbolReference, newIncludeDisabled: boolean, newIncludeObsolete: boolean, newIncludeNormal: boolean, newIncludeFlowFields: boolean, newIncludeFlowFilters: boolean, newIncludeToolTips: boolean, newToolTipsSourceDependencies: string[] | undefined) {
        super(newPath, true);
        this.tableReference = newTableReference;
        this.includeDisabled = newIncludeDisabled;
        this.includeObsolete = newIncludeObsolete;
        this.includeNormal = newIncludeNormal;
        this.includeFlowFields = newIncludeFlowFields;
        this.includeFlowFilters = newIncludeFlowFilters;
        this.includeToolTips = newIncludeToolTips;
        this.toolTipsSourceDependencies = newToolTipsSourceDependencies;
    }

}