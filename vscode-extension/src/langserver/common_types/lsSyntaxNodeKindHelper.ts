import { LSSyntaxNodeAccessModifier } from "./lsSyntaxNodeAccessModifier";
import { LSSyntaxNodeKind } from "./lsSyntaxNodeKind";

export class LSSyntaxNodeKindHelper {

    static toDescription(kind: LSSyntaxNodeKind): string {
        switch (kind) {
            case LSSyntaxNodeKind.Undefined: return "Undefined";
            case LSSyntaxNodeKind.CompilationUnit: return "CompilationUnit";
            case LSSyntaxNodeKind.PropertyList: return "Properties";
            case LSSyntaxNodeKind.ParameterList: return "parameters";
            case LSSyntaxNodeKind.VarSection: return "var";
            case LSSyntaxNodeKind.TriggerDeclaration: return "trigger";
            case LSSyntaxNodeKind.EventTriggerDeclaration: return "event trigger";
            case LSSyntaxNodeKind.MethodDeclaration: return "procedure";
            case LSSyntaxNodeKind.EventDeclaration: return "event";
            case LSSyntaxNodeKind.Parameter: return "parameter";
            case LSSyntaxNodeKind.VariableDeclaration: return "variable";
            case LSSyntaxNodeKind.FieldList: return "fields";
            case LSSyntaxNodeKind.Field: return "Field";
            case LSSyntaxNodeKind.DotNetAssembly: return "DotNetAssembly";
            case LSSyntaxNodeKind.DotNetTypeDeclaration: return "DotNetTypeDeclaration";
            case LSSyntaxNodeKind.FieldExtensionList: return "FieldExtensionList";
            case LSSyntaxNodeKind.FieldModification: return "FieldModification";
            case LSSyntaxNodeKind.KeyList: return "keys";
            case LSSyntaxNodeKind.Key: return "Key";
            case LSSyntaxNodeKind.FieldGroupList: return "fieldgroups";
            case LSSyntaxNodeKind.FieldGroup: return "FieldGroup";
            case LSSyntaxNodeKind.PageLayout: return "layout";
            case LSSyntaxNodeKind.PageActionList: return "actions";
            case LSSyntaxNodeKind.GroupActionList: return "groupactions";
            case LSSyntaxNodeKind.PageArea: return "Area";
            case LSSyntaxNodeKind.PageGroup: return "Group";
            case LSSyntaxNodeKind.PageField: return "Field";
            case LSSyntaxNodeKind.PageLabel: return "Label";
            case LSSyntaxNodeKind.PagePart: return "Part";
            case LSSyntaxNodeKind.PageSystemPart: return "SystemPart";
            case LSSyntaxNodeKind.PageChartPart: return "ChartPart";
            case LSSyntaxNodeKind.PageUserControl: return "UserControl";
            case LSSyntaxNodeKind.PageAction: return "Action";
            case LSSyntaxNodeKind.PageActionGroup: return "Group";
            case LSSyntaxNodeKind.PageActionArea: return "Area";
            case LSSyntaxNodeKind.PageActionSeparator: return "Separator";
            case LSSyntaxNodeKind.PageExtensionActionList: return "actions";
            case LSSyntaxNodeKind.ActionAddChange: return "AddChange";
            case LSSyntaxNodeKind.ActionMoveChange: return "MoveChange";
            case LSSyntaxNodeKind.ActionModifyChange: return "ModifyChange";
            case LSSyntaxNodeKind.PageExtensionLayout: return "Layout";
            case LSSyntaxNodeKind.ControlAddChange: return "AddChange";
            case LSSyntaxNodeKind.ControlMoveChange: return "MoveChange";
            case LSSyntaxNodeKind.ControlModifyChange: return "ModifyChange";
            case LSSyntaxNodeKind.PageExtensionViewList: return "Views";
            case LSSyntaxNodeKind.ViewAddChange: return "AddChange";
            case LSSyntaxNodeKind.ViewMoveChange: return "MoveChange";
            case LSSyntaxNodeKind.ViewModifyChange: return "ModifyChange";
            case LSSyntaxNodeKind.ReportDataSetSection: return "dataset";
            case LSSyntaxNodeKind.ReportLabelsSection: return "labels";
            case LSSyntaxNodeKind.ReportDataItem: return "Data Item";
            case LSSyntaxNodeKind.ReportColumn: return "Column";
            case LSSyntaxNodeKind.ReportLabel: return "Label";
            case LSSyntaxNodeKind.ReportLabelMultilanguage: return "Label";
            case LSSyntaxNodeKind.XmlPortSchema: return "schema";
            case LSSyntaxNodeKind.XmlPortTableElement: return "TableElement";
            case LSSyntaxNodeKind.XmlPortFieldElement: return "Field";
            case LSSyntaxNodeKind.XmlPortTextElement: return "TextElement";
            case LSSyntaxNodeKind.XmlPortFieldAttribute: return "Attribute";
            case LSSyntaxNodeKind.XmlPortTextAttribute: return "TextAttribute";
            case LSSyntaxNodeKind.RequestPage: return "RequestOptionsPage";
            case LSSyntaxNodeKind.QueryElements: return "elements";
            case LSSyntaxNodeKind.QueryDataItem: return "Data Item";
            case LSSyntaxNodeKind.QueryColumn: return "Column";
            case LSSyntaxNodeKind.QueryFilter: return "Filter";
            case LSSyntaxNodeKind.EnumType: return "EnumType";
            case LSSyntaxNodeKind.EnumValue: return "EnumValue";
            case LSSyntaxNodeKind.EnumExtensionType: return "EnumExtensionType";
            case LSSyntaxNodeKind.PageViewList: return "PageViewList";
            case LSSyntaxNodeKind.PageView: return "PageView";
            case LSSyntaxNodeKind.CodeunitObject: return "Codeunit";
            case LSSyntaxNodeKind.TableObject: return "Table";
            case LSSyntaxNodeKind.TableExtensionObject: return "TableExtension";
            case LSSyntaxNodeKind.PageObject: return "Page";
            case LSSyntaxNodeKind.PageExtensionObject: return "PageExtension";
            case LSSyntaxNodeKind.ReportObject: return "Report";
            case LSSyntaxNodeKind.XmlPortObject: return "XmlPort";
            case LSSyntaxNodeKind.QueryObject: return "Query";
            case LSSyntaxNodeKind.ControlAddInObject: return "ControlAddIn";
            case LSSyntaxNodeKind.ProfileObject: return "Profile";
            case LSSyntaxNodeKind.PageCustomizationObject: return "PageCustomization";
            case LSSyntaxNodeKind.DotNetPackage: return "DotNetPackage";
            case LSSyntaxNodeKind.GlobalVarSection: return "var";
            case LSSyntaxNodeKind.VariableDeclarationName: return "var";
            case LSSyntaxNodeKind.Entitlement: return "Entitlement";
            case LSSyntaxNodeKind.PermissionSet: return "PermissionSet";
            case LSSyntaxNodeKind.PermissionSetExtension: return "PermissionSetExtension";
            case LSSyntaxNodeKind.ReportExtensionAddColumnChange: return "AddColumn";
            case LSSyntaxNodeKind.ReportExtensionAddDataItemChange: return "AddDataItem";
            case LSSyntaxNodeKind.ReportExtensionDataSetAddColumn: return "AddDataSetColumn";
            case LSSyntaxNodeKind.ReportExtensionDataSetAddDataItem: return "AddDataItem";
            case LSSyntaxNodeKind.ReportExtensionDataSetModify: return "ModifyDataSet";
            case LSSyntaxNodeKind.ReportExtensionDataSetSection: return "dataset";
            case LSSyntaxNodeKind.ReportExtensionModifyChange: return "Modify";
            case LSSyntaxNodeKind.ReportExtensionObject: return "Report Extension";
            case LSSyntaxNodeKind.RequestPageExtension: return "RequestOptionsPage";
            case LSSyntaxNodeKind.LocalMethodDeclaration: return "local procedure";
            case LSSyntaxNodeKind.InternalMethodDeclaration: return "internal procedure";
            case LSSyntaxNodeKind.ProtectedMethodDeclaration: return "protected procedure";
            case LSSyntaxNodeKind.PrimaryKey: return "Primary Key";
            case LSSyntaxNodeKind.Module: return "Module";
            case LSSyntaxNodeKind.TableObjectList: return "Tables";
            case LSSyntaxNodeKind.PageObjectList: return "Pages";
            case LSSyntaxNodeKind.ReportObjectList: return "Reports";
            case LSSyntaxNodeKind.XmlPortObjectList: return "XmlPorts";
            case LSSyntaxNodeKind.QueryObjectList: return "Queries";
            case LSSyntaxNodeKind.CodeunitObjectList: return "Codeunits";
            case LSSyntaxNodeKind.ControlAddInObjectList: return "ControlAddIns";
            case LSSyntaxNodeKind.PageExtensionObjectList: return "PageExtensions";
            case LSSyntaxNodeKind.TableExtensionObjectList: return "TableExtensions";
            case LSSyntaxNodeKind.ProfileObjectList: return "Profiles";
            case LSSyntaxNodeKind.PageCustomizationObjectList: return "PageCustomizations";
            case LSSyntaxNodeKind.EnumObjectList: return "Enums";
            case LSSyntaxNodeKind.DotNetPackageList: return "DotNetPackages";
            case LSSyntaxNodeKind.EnumTypeList: return "Enums";
            case LSSyntaxNodeKind.EnumExtensionTypeList: return "EnumExtensions";
            case LSSyntaxNodeKind.InterfaceObjectList: return "Interfaces";
            case LSSyntaxNodeKind.ReportExtensionObjectList: return "ReportExtensions";
            case LSSyntaxNodeKind.PermissionSetList: return "PermissionSets";
            case LSSyntaxNodeKind.PermissionSetExtensionList: return "PermissionSetExtensions";
            case LSSyntaxNodeKind.EntitlementList: return "Entitlements";
            case LSSyntaxNodeKind.ProfileExtensionObject: return "ProfileExtensions";
            case LSSyntaxNodeKind.Namespace: return "Namespace";
            case LSSyntaxNodeKind.Package: return "Package";
            case LSSyntaxNodeKind.Class: return "Class";
            case LSSyntaxNodeKind.Property: return "Property";
            case LSSyntaxNodeKind.Constructor: return "Constructor";
            case LSSyntaxNodeKind.Interface: return "Interface";
            case LSSyntaxNodeKind.Constant: return "Constant";
            case LSSyntaxNodeKind.String: return "String";
            case LSSyntaxNodeKind.Number: return "Number";
            case LSSyntaxNodeKind.Boolean: return "Boolean";
            case LSSyntaxNodeKind.Array: return "Array";
            case LSSyntaxNodeKind.Null: return "Null";
            case LSSyntaxNodeKind.Object: return "Object";
            case LSSyntaxNodeKind.Struct: return "Struct";
            case LSSyntaxNodeKind.Operator: return "Operator";
            case LSSyntaxNodeKind.PageRepeater: return "Repeater";
            case LSSyntaxNodeKind.IntegrationEventDeclaration: return "Integration Event";
            case LSSyntaxNodeKind.BusinessEventDeclaration: return "Business Event";
            case LSSyntaxNodeKind.EventSubscriberDeclaration: return "Event Subscriber";
            case LSSyntaxNodeKind.InternalEventDeclaration: return "Internal Event";
            case LSSyntaxNodeKind.ExternalBusinessEventDeclaration: return "External Business Event";
            case LSSyntaxNodeKind.TestDeclaration: return "Test";
            case LSSyntaxNodeKind.ConfirmHandlerDeclaration: return "Confirm Handler";
            case LSSyntaxNodeKind.FilterPageHandlerDeclaration: return "Filter Page Handler";
            case LSSyntaxNodeKind.HyperlinkHandlerDeclaration: return "Hyperlink Handler";
            case LSSyntaxNodeKind.MessageHandlerDeclaration: return "Message Handler";
            case LSSyntaxNodeKind.ModalPageHandlerDeclaration: return "Modal Page Handler";
            case LSSyntaxNodeKind.PageHandlerDeclaration: return "Page Handler";
            case LSSyntaxNodeKind.ReportHandlerDeclaration: return "Report Handler";
            case LSSyntaxNodeKind.RequestPageHandlerDeclaration: return "Request Page Handler";
            case LSSyntaxNodeKind.SendNotificationHandlerDeclaration: return "Send Notification Handler";
            case LSSyntaxNodeKind.SessionSettingsHandlerDeclaration: return "Session Settings Handler";
            case LSSyntaxNodeKind.StrMenuHandlerDeclaration: return "StrMenu Handler";
            case LSSyntaxNodeKind.ProjectDefinition: return "Project";
            case LSSyntaxNodeKind.PackagesList: return "Packages";
            case LSSyntaxNodeKind.Dependencies: return "Dependencies";
            case LSSyntaxNodeKind.Document: return "Document";
            case LSSyntaxNodeKind.SymbolGroup: return "SymbolGroup";
            case LSSyntaxNodeKind.AnyALObject: return "AL Object";
            case LSSyntaxNodeKind.SyntaxTreeNode: return "SyntaxTreeNode";
            case LSSyntaxNodeKind.SyntaxTreeToken: return "SyntaxTreeToken";
            case LSSyntaxNodeKind.SyntaxTreeTrivia: return "SyntaxTreeTrivia";
            case LSSyntaxNodeKind.Region: return "Region";
            case LSSyntaxNodeKind.UsingDirective: return "Using";
            case LSSyntaxNodeKind.FieldGroupAddChange: return "FieldGroupAddChange";
            case LSSyntaxNodeKind.FieldGroupExtensionList: return "FieldGroupExtensionList";
            case LSSyntaxNodeKind.PageActionRef: return "PageActionRef";
            case LSSyntaxNodeKind.PageCustomAction: return "PageCustomAction";
            case LSSyntaxNodeKind.PageFieldUploadAction: return "PageFieldUploadAction";
            case LSSyntaxNodeKind.PageSystemAction: return "PageSystemAction";
            case LSSyntaxNodeKind.ReportLayout: return "ReportLayout";
            case LSSyntaxNodeKind.ReportRenderingSection: return "ReportRenderingSection";
            case LSSyntaxNodeKind.ProfileExtensionObjectList: return "ProfileExtensionsList";
            case LSSyntaxNodeKind.Permission: return "Permission";
            default: return "Unknown";
        }
    }

    static isALObject(kind: LSSyntaxNodeKind): boolean {
        switch (kind) {
            case LSSyntaxNodeKind.TableObject:
            case LSSyntaxNodeKind.CodeunitObject:
            case LSSyntaxNodeKind.PageObject:
            case LSSyntaxNodeKind.ReportObject:
            case LSSyntaxNodeKind.QueryObject:
            case LSSyntaxNodeKind.XmlPortObject:
            case LSSyntaxNodeKind.TableExtensionObject:
            case LSSyntaxNodeKind.PageExtensionObject:
            case LSSyntaxNodeKind.ControlAddInObject:
            case LSSyntaxNodeKind.EnumType:
            case LSSyntaxNodeKind.EnumExtensionType:
            case LSSyntaxNodeKind.DotNetPackage:
            case LSSyntaxNodeKind.ProfileObject:
            case LSSyntaxNodeKind.PageCustomizationObject:
            case LSSyntaxNodeKind.Interface:
            case LSSyntaxNodeKind.ReportExtensionObject:
            case LSSyntaxNodeKind.PermissionSet:
            case LSSyntaxNodeKind.PermissionSetExtension:
            case LSSyntaxNodeKind.Entitlement:
                return true;
        }
        return false;
    }

    public static getIconName(kind: LSSyntaxNodeKind, access: LSSyntaxNodeAccessModifier | undefined, subtype: string | undefined): string {
        switch (kind) {
            case LSSyntaxNodeKind.Class : return 'class';
            case LSSyntaxNodeKind.Package : return 'module';
            case LSSyntaxNodeKind.SymbolGroup : return 'module';
            case LSSyntaxNodeKind.Undefined : return 'undefined';
            case LSSyntaxNodeKind.TableObject : return 'table';
            case LSSyntaxNodeKind.CodeunitObject : return 'codeunit';
            case LSSyntaxNodeKind.PageObject : return 'page';
            case LSSyntaxNodeKind.ReportObject : return 'report';
            case LSSyntaxNodeKind.QueryObject : return 'query';
            case LSSyntaxNodeKind.XmlPortObject : return 'xmlport';
            case LSSyntaxNodeKind.TableExtensionObject : return 'tableextension';
            case LSSyntaxNodeKind.PageExtensionObject : return 'pageextension';
            case LSSyntaxNodeKind.ControlAddInObject : return 'controladdin';
            case LSSyntaxNodeKind.ProfileObject : return 'profile';
            case LSSyntaxNodeKind.PageCustomizationObject : return 'pagecustomization';
            case LSSyntaxNodeKind.EnumType : return 'enum';
            case LSSyntaxNodeKind.DotNetPackage : return 'dotnetlib';

            case LSSyntaxNodeKind.ReportExtensionObject: return 'report';
            case LSSyntaxNodeKind.PermissionSet: return 'profile';
            case LSSyntaxNodeKind.PermissionSetExtension: return 'profile';
            case LSSyntaxNodeKind.Entitlement: return 'profile';

            case LSSyntaxNodeKind.Interface: return 'interface';
            case LSSyntaxNodeKind.Property : return 'property';
            case LSSyntaxNodeKind.VariableDeclaration : 
                if (access === LSSyntaxNodeAccessModifier.Protected) {
                    return 'variableprotected';
                }
                return 'variable';
            case LSSyntaxNodeKind.VariableDeclarationName : 
                if (access === LSSyntaxNodeAccessModifier.Protected) {
                    return 'variableprotected';
                }
                return 'variable';
            case LSSyntaxNodeKind.Constant : return 'constant';
            case LSSyntaxNodeKind.Parameter : return 'parameter';
            case LSSyntaxNodeKind.VarSection: return 'variablelist';
            case LSSyntaxNodeKind.GlobalVarSection: 
                if (access === LSSyntaxNodeAccessModifier.Protected) {
                    return 'variablelistprotected';
                }
                return 'variablelist';
            case LSSyntaxNodeKind.MethodDeclaration : return 'method';            
            case LSSyntaxNodeKind.LocalMethodDeclaration : return 'methodprivate';
            case LSSyntaxNodeKind.ProtectedMethodDeclaration : return 'methodprotected';
            case LSSyntaxNodeKind.InternalMethodDeclaration : return 'methodinternal';
            case LSSyntaxNodeKind.TriggerDeclaration : return 'trigger';
            case LSSyntaxNodeKind.Region: return 'region';

            case LSSyntaxNodeKind.ParameterList: return 'parameterlist';
            case LSSyntaxNodeKind.PropertyList: return 'propertieslist';

            //events
            case LSSyntaxNodeKind.IntegrationEventDeclaration: return 'integrationevent';
            case LSSyntaxNodeKind.InternalEventDeclaration: return 'integrationevent';
            case LSSyntaxNodeKind.BusinessEventDeclaration: return 'businessevent';
            case LSSyntaxNodeKind.ExternalBusinessEventDeclaration: return 'businessevent';
            case LSSyntaxNodeKind.EventSubscriberDeclaration: return 'eventsubscriber';
            //tests
            case LSSyntaxNodeKind.TestDeclaration: return 'test';
            case LSSyntaxNodeKind.ConfirmHandlerDeclaration: return 'testcontroller';
            case LSSyntaxNodeKind.FilterPageHandlerDeclaration: return 'testcontroller';
            case LSSyntaxNodeKind.HyperlinkHandlerDeclaration: return 'testcontroller';
            case LSSyntaxNodeKind.MessageHandlerDeclaration: return 'testcontroller';
            case LSSyntaxNodeKind.ModalPageHandlerDeclaration: return 'testcontroller';
            case LSSyntaxNodeKind.PageHandlerDeclaration: return 'testcontroller';
            case LSSyntaxNodeKind.ReportHandlerDeclaration: return 'testcontroller';
            case LSSyntaxNodeKind.RequestPageHandlerDeclaration: return 'testcontroller';
            case LSSyntaxNodeKind.SendNotificationHandlerDeclaration: return 'testcontroller';
            case LSSyntaxNodeKind.SessionSettingsHandlerDeclaration: return 'testcontroller';
            case LSSyntaxNodeKind.StrMenuHandlerDeclaration: return 'testcontroller';

            case LSSyntaxNodeKind.Field : 
                if (subtype === 'ObsoletePending') {                    
                    return 'fieldpending';
                }
                if (subtype === 'ObsoleteRemoved') {                    
                    return 'fieldobsolete';
                }
                if (subtype === "Disabled") {
                    return "fielddisabled";
                }
                return 'field';
            case LSSyntaxNodeKind.PrimaryKey : return 'primarykey';
            case LSSyntaxNodeKind.Key : return 'key';
            case LSSyntaxNodeKind.FieldGroup : return 'fieldgroup';
            case LSSyntaxNodeKind.PageArea: return 'group';
            case LSSyntaxNodeKind.PageGroup : return 'group';
            case LSSyntaxNodeKind.PageRepeater: return 'repeater';
            case LSSyntaxNodeKind.PagePart: return 'pagepart';
            case LSSyntaxNodeKind.PageChartPart: return 'chartpart';
            case LSSyntaxNodeKind.PageSystemPart: return 'systempart';
            case LSSyntaxNodeKind.PageLayout: return 'pagelayout';
            case LSSyntaxNodeKind.PageActionList: return 'pageactions';
            case LSSyntaxNodeKind.PageLabel: return 'label';
            
            case LSSyntaxNodeKind.PageActionGroup: return 'group';
            case LSSyntaxNodeKind.PageActionArea: return 'group';
            case LSSyntaxNodeKind.PageAction : return 'action';
            case LSSyntaxNodeKind.EnumValue : return 'enumval';
            case LSSyntaxNodeKind.EnumExtensionType : return 'enumext';
            case LSSyntaxNodeKind.DotNetAssembly : return 'dotnetasm';
            case LSSyntaxNodeKind.DotNetTypeDeclaration : return 'dotnetclass';

            case LSSyntaxNodeKind.PageField: return 'field';
            case LSSyntaxNodeKind.FieldModification: return 'field';

            case LSSyntaxNodeKind.EventDeclaration: return 'event';
            case LSSyntaxNodeKind.EventTriggerDeclaration: return 'trigger';

            case LSSyntaxNodeKind.XmlPortSchema: return 'codeunit';
            case LSSyntaxNodeKind.XmlPortTableElement: return 'table';
            case LSSyntaxNodeKind.XmlPortFieldElement: return 'variable';
            case LSSyntaxNodeKind.XmlPortTextElement: return 'variable';
            case LSSyntaxNodeKind.XmlPortFieldAttribute: return 'parameter';
            case LSSyntaxNodeKind.XmlPortTextAttribute: return 'parameter';
            case LSSyntaxNodeKind.RequestPage: return 'page';
        
            //reports
            case LSSyntaxNodeKind.ReportDataSetSection: return 'codeunit';
            case LSSyntaxNodeKind.ReportLabelsSection: return 'codeunit';
            case LSSyntaxNodeKind.ReportDataItem: return 'table';
            case LSSyntaxNodeKind.ReportColumn: return 'field';
            case LSSyntaxNodeKind.ReportLabel: return 'variable';
            case LSSyntaxNodeKind.ReportLabelMultilanguage: return 'variable';
        
            //queries
            case LSSyntaxNodeKind.QueryElements: return 'codeunit';
            case LSSyntaxNodeKind.QueryDataItem: return 'table';
            case LSSyntaxNodeKind.QueryColumn: return 'field';
            case LSSyntaxNodeKind.QueryFilter: return 'parameter';

            //groups                       
            case LSSyntaxNodeKind.KeyList:
            case LSSyntaxNodeKind.FieldList:
            case LSSyntaxNodeKind.FieldGroupList:
            case LSSyntaxNodeKind.FieldExtensionList:
            case LSSyntaxNodeKind.PageViewList:
            case LSSyntaxNodeKind.GroupActionList:
            case LSSyntaxNodeKind.PageExtensionViewList:
            case LSSyntaxNodeKind.PageExtensionActionList:
                return 'codeunit';
        }
        return 'undefined';
    }


}