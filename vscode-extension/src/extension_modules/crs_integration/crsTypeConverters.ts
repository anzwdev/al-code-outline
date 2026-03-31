import { LSObjectKind } from "../../langserver/common_types/lsObjectKind";
import { CrsOnSaveAlFileAction } from "./crsOnSaveAlFileAction";

export class CrsTypeConverters {

    public static parseCrsOnSaveAlFileAction(value: string | undefined) : CrsOnSaveAlFileAction {
        switch(value) {
            case "Rename": return CrsOnSaveAlFileAction.rename;
            case "Reorganize": return CrsOnSaveAlFileAction.reorganize;
            case "DoNothing":
            default:
                return CrsOnSaveAlFileAction.doNothing;
        }
    }

    public static objectKindToObjectTypeName(symbolKind : LSObjectKind) {
        switch (symbolKind) {
            case LSObjectKind.Table: return 'table';
            case LSObjectKind.Codeunit: return 'codeunit';
            case LSObjectKind.Page: return 'page';
            case LSObjectKind.Report: return 'report';
            case LSObjectKind.Query: return 'query';
            case LSObjectKind.XmlPort: return 'xmlport';
            case LSObjectKind.TableExtension: return 'tableextension';
            case LSObjectKind.PageExtension: return 'pageextension';
            case LSObjectKind.ControlAddIn: return 'controladdin';
            case LSObjectKind.Profile: return 'profile';
            case LSObjectKind.PageCustomization: return 'pagecustomization';
            case LSObjectKind.EnumType: return 'enum';
            case LSObjectKind.DotNetPackage: return 'dotnetpackage';
            case LSObjectKind.EnumExtensionType: return 'enumextension';
            case LSObjectKind.Interface: return 'interface';
            case LSObjectKind.ReportExtension: return 'reportextension';
            case LSObjectKind.PermissionSet: return 'permissionset';
            case LSObjectKind.PermissionSetExtension: return 'permissionsetextension';
            case LSObjectKind.Entitlement: return 'entitlement';
       }
       return '';
    }

}