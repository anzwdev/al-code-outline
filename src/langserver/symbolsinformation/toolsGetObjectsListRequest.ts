import { ToolsSymbolInformationRequest } from "./toolsSymbolInformationRequest";

export class ToolsGetObjectsListRequest extends ToolsSymbolInformationRequest {
    includeTables : boolean;
    includePages : boolean;
    includeReports : boolean;
    includeXmlPorts : boolean;
    includeQueries : boolean;
    includeCodeunits : boolean;
    includeControlAddIns : boolean;
    includePageExtensions : boolean;
    includeTableExtensions : boolean;
    includePofiles : boolean;
    includePageCustomizations : boolean;
    includeDotNetPackages : boolean;
    includeEnumTypes : boolean;
    includeEnumExtensionTypes : boolean;
    includeInterfaces : boolean;
    includeReportExtensions : boolean;
    includePermissionSets : boolean;
    includePermissionSetExtensions : boolean;
    
    includeDependencies : boolean;

    constructor(newPath: string | undefined) {
        super(newPath, false);
        this.includeTables = false;
        this.includePages = false;
        this.includeReports = false;
        this.includeXmlPorts = false;
        this.includeQueries = false;
        this.includeCodeunits = false;
        this.includeControlAddIns = false;
        this.includePageExtensions = false;
        this.includeTableExtensions = false;
        this.includePofiles = false;
        this.includePageCustomizations = false;
        this.includeDotNetPackages = false;
        this.includeEnumTypes = false;
        this.includeEnumExtensionTypes = false;
        this.includeInterfaces = false;
        this.includeReportExtensions = false;
        this.includePermissionSets = false;
        this.includePermissionSetExtensions = false;

        this.includeDependencies = false;
    }

    setIncludeAll() {
        this.includeTables = true;
        this.includePages = true;
        this.includeReports = true;
        this.includeXmlPorts = true;
        this.includeQueries = true;
        this.includeCodeunits = true;
        this.includeControlAddIns = true;
        this.includePageExtensions = true;
        this.includeTableExtensions = true;
        this.includePofiles = true;
        this.includePageCustomizations = true;
        this.includeDotNetPackages = true;
        this.includeEnumTypes = true;
        this.includeEnumExtensionTypes = true;
        this.includeInterfaces = true;
        this.includeReportExtensions = true;
        this.includePermissionSets = true;
        this.includePermissionSetExtensions = true;        
    }

    setIncludeObjectsWithPermissions() {
        this.includeTables = true;
        this.includePages = true;
        this.includeReports = true;
        this.includeXmlPorts = true;
        this.includeQueries = true;
        this.includeCodeunits = true;
        this.includeControlAddIns = false;
        this.includePageExtensions = false;
        this.includeTableExtensions = false;
        this.includePofiles = false;
        this.includePageCustomizations = false;
        this.includeDotNetPackages = false;
        this.includeEnumTypes = false;
        this.includeEnumExtensionTypes = false;
        this.includeInterfaces = false;
        this.includeReportExtensions = false;
        this.includePermissionSets = false;
        this.includePermissionSetExtensions = false;
    }

}