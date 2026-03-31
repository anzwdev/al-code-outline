import { LSObjectKind } from "./lsObjectKind";

export class LSObjectKindHelper {

    static fromString(value: string, defaultKind: LSObjectKind): LSObjectKind {
        switch (value.toLowerCase()) {
            case "record":
            case "table": 
                return LSObjectKind.Table;
            case "page": 
                return LSObjectKind.Page;
            case "report": 
                return LSObjectKind.Report;
            case "xmlport": 
                return LSObjectKind.XmlPort;
            case "query": 
                return LSObjectKind.Query;
            case "codeunit": 
                return LSObjectKind.Codeunit;
            case "controladdin": 
                return LSObjectKind.ControlAddIn;
            case "pageextension": 
                return LSObjectKind.PageExtension;
            case "tableextension": 
                return LSObjectKind.TableExtension;
            case "profile": 
                return LSObjectKind.Profile;
            case "profileextension": 
                return LSObjectKind.ProfileExtension;
            case "pagecustomization": 
                return LSObjectKind.PageCustomization;
            case "dotnetpackage": 
                return LSObjectKind.DotNetPackage;
            case "enum":
            case "enumtype": 
                return LSObjectKind.EnumType;
            case "enumextension":
            case "enumextensiontype": 
                return LSObjectKind.EnumExtensionType;
            case "interface": 
                return LSObjectKind.Interface;
            case "reportextension": 
                return LSObjectKind.ReportExtension;
            case "permissionset": 
                return LSObjectKind.PermissionSet;
            case "permissionsetextension": 
                return LSObjectKind.PermissionSetExtension;
            case "entitlement": 
                return LSObjectKind.Entitlement;
        }
        return defaultKind;
    }

    static toObjectTypeName(kind: LSObjectKind): string | undefined {
        switch (kind) {
        case LSObjectKind.Table:
            return "Table";
        case LSObjectKind.Page:
            return "Page";
        case LSObjectKind.Report:
            return "Report";
        case LSObjectKind.XmlPort:
            return "XmlPort";
        case LSObjectKind.Query:
            return "Query";
        case LSObjectKind.Codeunit:
            return "Codeunit";
        case LSObjectKind.ControlAddIn:
            return "ControlAddIn";
        case LSObjectKind.PageExtension:
            return "PageExtension";
        case LSObjectKind.TableExtension:
            return "TableExtension";
        case LSObjectKind.Profile:
            return "Profile";
        case LSObjectKind.ProfileExtension:
            return "ProfileExtension";
        case LSObjectKind.PageCustomization:
            return "PageCustomization";
        case LSObjectKind.DotNetPackage:
            return "DotNetPackage";
        case LSObjectKind.EnumType:
            return "EnumType";
        case LSObjectKind.EnumExtensionType:
            return "EnumExtensionType";
        case LSObjectKind.Interface:
            return "Interface";
        case LSObjectKind.ReportExtension:
            return "ReportExtension";
        case LSObjectKind.PermissionSet:
            return "PermissionSet";
        case LSObjectKind.PermissionSetExtension:
            return "PermissionSetExtension";
        case LSObjectKind.Entitlement:
            return "Entitlement";
        case LSObjectKind.System:
            return "System";
        case LSObjectKind.SystemPart:
            return "SystemPart";
        case LSObjectKind.Chart:
            return "Chart";
        case LSObjectKind.TableData:
            return "TableData";
        case LSObjectKind.Unknown:
            return undefined;
        }
        return undefined;
    }

    static toVariableTypeName(kind: LSObjectKind): string | undefined {
        switch (kind) {
            case LSObjectKind.Table:
                return "record";
            case LSObjectKind.Codeunit:
                return "codeunit";
            case LSObjectKind.Page:
                return "page";
            case LSObjectKind.Report:
                return "report";
            case LSObjectKind.Query:
                return "query";
            case LSObjectKind.XmlPort:
                return "xmlport";
            case LSObjectKind.ControlAddIn:
                return "usercontrol";
            case LSObjectKind.EnumType:
                return "enum";
            case LSObjectKind.Interface:
                return "interface";
        }
        return undefined;
    }

}