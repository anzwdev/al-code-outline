using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols
{
    public static class ALSymbolKindExtension
    {

        public static string ToName(this ALSymbolKind value)
        {
            switch (value)
            {
                case ALSymbolKind.Undefined: return "Undefined";
                //case ALSymbolKind.//BooleanLiteralValue: return "//BooleanLiteralValue";
                //case ALSymbolKind.//Int32SignedLiteralValue: return "//Int32SignedLiteralValue";
                //case ALSymbolKind.//Int64SignedLiteralValue: return "//Int64SignedLiteralValue";
                //case ALSymbolKind.//DecimalSignedLiteralValue: return "//DecimalSignedLiteralValue";
                //case ALSymbolKind.//DateLiteralValue: return "//DateLiteralValue";
                //case ALSymbolKind.//TimeLiteralValue: return "//TimeLiteralValue";
                //case ALSymbolKind.//DateTimeLiteralValue: return "//DateTimeLiteralValue";
                //case ALSymbolKind.//StringLiteralValue: return "//StringLiteralValue";
                //case ALSymbolKind.//IdentifierName: return "//IdentifierName";
                //case ALSymbolKind.//QualifiedName: return "//QualifiedName";
                //case ALSymbolKind.//IdentifierNameOrEmpty: return "//IdentifierNameOrEmpty";
                //case ALSymbolKind.//ArgumentList: return "//ArgumentList";
                //case ALSymbolKind.//BracketedArgumentList: return "//BracketedArgumentList";
                case ALSymbolKind.CompilationUnit: return "CompilationUnit";
                //case ALSymbolKind.//PropertyList: return "//PropertyList";
                //case ALSymbolKind.//OptionValues: return "//OptionValues";
                //case ALSymbolKind.//ObjectReference: return "//ObjectReference";
                //case ALSymbolKind.//ObjectId: return "//ObjectId";
                //case ALSymbolKind.//ObjectNameReference: return "//ObjectNameReference";
                case ALSymbolKind.ParameterList: return "parameters";
                //case ALSymbolKind.//MethodBody: return "//MethodBody";
                case ALSymbolKind.VarSection: return "var";
                case ALSymbolKind.GlobalVarSection: return "var";
                case ALSymbolKind.TriggerDeclaration: return "trigger";
                case ALSymbolKind.EventTriggerDeclaration: return "event trigger";
                case ALSymbolKind.LocalMethodDeclaration: return "local procedure";
                case ALSymbolKind.InternalMethodDeclaration: return "internal procedure";
                case ALSymbolKind.ProtectedMethodDeclaration: return "protected procedure";
                case ALSymbolKind.MethodDeclaration: return "procedure";
                case ALSymbolKind.EventDeclaration: return "event";
                case ALSymbolKind.Parameter: return "parameter";
                case ALSymbolKind.VariableDeclaration: return "variable";
                //case ALSymbolKind.//ReturnValue: return "//ReturnValue";
                //case ALSymbolKind.//SimpleTypeReference: return "//SimpleTypeReference";
                //case ALSymbolKind.//RecordTypeReference: return "//RecordTypeReference";
                //case ALSymbolKind.//DotNetTypeReference: return "//DotNetTypeReference";
                //case ALSymbolKind.//DataType: return "//DataType";
                //case ALSymbolKind.//GenericDataType: return "//GenericDataType";
                //case ALSymbolKind.//OptionDataType: return "//OptionDataType";
                //case ALSymbolKind.//TextConstDataType: return "//TextConstDataType";
                //case ALSymbolKind.//LabelDataType: return "//LabelDataType";
                //case ALSymbolKind.//DotNetDataType: return "//DotNetDataType";
                //case ALSymbolKind.//LengthDataType: return "//LengthDataType";
                //case ALSymbolKind.//SubtypedDataType: return "//SubtypedDataType";
                //case ALSymbolKind.//EnumDataType: return "//EnumDataType";
                //case ALSymbolKind.//Array: return "//Array";
                //case ALSymbolKind.//BracketedDimensionList: return "//BracketedDimensionList";
                //case ALSymbolKind.//Dimension: return "//Dimension";
                //case ALSymbolKind.//MemberAttribute: return "//MemberAttribute";
                case ALSymbolKind.FieldList: return "fields";
                case ALSymbolKind.Field: return "Field";
                case ALSymbolKind.DotNetAssembly: return "DotNetAssembly";
                case ALSymbolKind.DotNetTypeDeclaration: return "DotNetTypeDeclaration";
                case ALSymbolKind.FieldExtensionList: return "FieldExtensionList";
                case ALSymbolKind.FieldModification: return "FieldModification";
                case ALSymbolKind.KeyList: return "keys";
                case ALSymbolKind.Key: return "Key";
                case ALSymbolKind.FieldGroupList: return "fieldgroups";
                case ALSymbolKind.FieldGroup: return "FieldGroup";
                case ALSymbolKind.PageLayout: return "layout";
                case ALSymbolKind.PageActionList: return "actions";
                case ALSymbolKind.GroupActionList: return "groupactions";
                case ALSymbolKind.PageArea: return "Area";
                case ALSymbolKind.PageGroup: return "Group";
                case ALSymbolKind.PageField: return "Field";
                case ALSymbolKind.PageLabel: return "Label";
                case ALSymbolKind.PagePart: return "Part";
                case ALSymbolKind.PageSystemPart: return "SystemPart";
                case ALSymbolKind.PageChartPart: return "ChartPart";
                case ALSymbolKind.PageUserControl: return "UserControl";
                case ALSymbolKind.PageAction: return "Action";
                case ALSymbolKind.PageActionGroup: return "Group";
                case ALSymbolKind.PageActionArea: return "Area";
                case ALSymbolKind.PageActionSeparator: return "Separator";
                case ALSymbolKind.PageExtensionActionList: return "actions";
                case ALSymbolKind.ActionAddChange: return "AddChange";
                case ALSymbolKind.ActionMoveChange: return "MoveChange";
                case ALSymbolKind.ActionModifyChange: return "ModifyChange";
                case ALSymbolKind.PageExtensionLayout: return "Layout";
                case ALSymbolKind.ControlAddChange: return "AddChange";
                case ALSymbolKind.ControlMoveChange: return "MoveChange";
                case ALSymbolKind.ControlModifyChange: return "ModifyChange";
                case ALSymbolKind.PageExtensionViewList: return "Views";
                case ALSymbolKind.ViewAddChange: return "AddChange";
                case ALSymbolKind.ViewMoveChange: return "MoveChange";
                case ALSymbolKind.ViewModifyChange: return "ModifyChange";
                
                case ALSymbolKind.ReportDataSetSection: return "dataset";
                case ALSymbolKind.ReportLabelsSection: return "labels";
                case ALSymbolKind.ReportDataItem: return "Data Item";
                case ALSymbolKind.ReportColumn: return "Column";
                case ALSymbolKind.ReportLabel: return "Label";
                case ALSymbolKind.ReportLabelMultilanguage: return "Label";
                
                case ALSymbolKind.XmlPortSchema: return "schema";
                case ALSymbolKind.XmlPortTableElement: return "TableElement";
                case ALSymbolKind.XmlPortFieldElement: return "Field";
                case ALSymbolKind.XmlPortTextElement: return "TextElement";
                case ALSymbolKind.XmlPortFieldAttribute: return "Attribute";
                case ALSymbolKind.XmlPortTextAttribute: return "TextAttribute";
                case ALSymbolKind.RequestPage: return "RequestOptionsPage";

                case ALSymbolKind.QueryElements: return "elements";
                case ALSymbolKind.QueryDataItem: return "Data Item";
                case ALSymbolKind.QueryColumn: return "Column";
                case ALSymbolKind.QueryFilter: return "Filter";
                //case ALSymbolKind.//Label: return "//Label";

                case ALSymbolKind.EnumType: return "EnumType";
                case ALSymbolKind.EnumValue: return "EnumValue";
                case ALSymbolKind.EnumExtensionType: return "EnumExtensionType";
                //case ALSymbolKind.//FieldGroupExtensionList: return "//FieldGroupExtensionList";
                //case ALSymbolKind.//FieldGroupAddChange: return "//FieldGroupAddChange";

                case ALSymbolKind.PageViewList: return "PageViewList";
                case ALSymbolKind.PageView: return "PageView";

                case ALSymbolKind.ReportExtensionAddColumnChange: return "AddColumn";
                case ALSymbolKind.ReportExtensionAddDataItemChange: return "AddDataItem";
                case ALSymbolKind.ReportExtensionDataSetAddColumn: return "AddDataSetColumn";
                case ALSymbolKind.ReportExtensionDataSetAddDataItem: return "AddDataItem";
                case ALSymbolKind.ReportExtensionDataSetModify: return "ModifyDataSet";
                case ALSymbolKind.ReportExtensionDataSetSection: return "dataset";
                case ALSymbolKind.ReportExtensionModifyChange: return "Modify";
                case ALSymbolKind.RequestPageExtension: return "RequestOptionsPage";

                //case ALSymbolKind.//PageFieldReferencePropertyValue: return "//PageFieldReferencePropertyValue";

                case ALSymbolKind.CodeunitObject: return "Codeunit";
                case ALSymbolKind.TableObject: return "Table";
                case ALSymbolKind.TableExtensionObject: return "Table Extension";
                case ALSymbolKind.PageObject: return "Page";
                case ALSymbolKind.PageExtensionObject: return "Page Extension";
                case ALSymbolKind.ReportObject: return "Report";
                case ALSymbolKind.XmlPortObject: return "XmlPort";
                case ALSymbolKind.QueryObject: return "Query";
                case ALSymbolKind.ControlAddInObject: return "Control AddIn";
                case ALSymbolKind.ProfileObject: return "Profile";
                case ALSymbolKind.PageCustomizationObject: return "Page Customization";
                case ALSymbolKind.DotNetPackage: return "DotNet Package";
                case ALSymbolKind.Interface: return "Interface";
                case ALSymbolKind.ReportExtensionObject: return "Report Extension";
                case ALSymbolKind.PermissionSet: return "PermissionSet";
                case ALSymbolKind.PermissionSetExtension: return "PermissionSet Extension";

                //case ALSymbolKind.//AttributeArgumentList: return "//AttributeArgumentList";
                //case ALSymbolKind.//LiteralAttributeArgument: return "//LiteralAttributeArgument";
                //case ALSymbolKind.//MethodReferenceAttributeArgument: return "//MethodReferenceAttributeArgument";
                //case ALSymbolKind.//OptionAccessAttributeArgument: return "//OptionAccessAttributeArgument";
                //case ALSymbolKind.//InvalidAttributeArgument: return "//InvalidAttributeArgument";

                case ALSymbolKind.PrimaryKey: return "Primary Key";
                case ALSymbolKind.PageRepeater: return "Repeater";

                case ALSymbolKind.TableObjectList: return "Tables";
                case ALSymbolKind.PageObjectList:return "Pages";
                case ALSymbolKind.ReportObjectList: return "Reports";
                case ALSymbolKind.XmlPortObjectList: return "XmlPorts";
                case ALSymbolKind.QueryObjectList: return "Queries";
                case ALSymbolKind.CodeunitObjectList: return "Codeunits";
                case ALSymbolKind.ControlAddInObjectList: return "ControlAddIns";
                case ALSymbolKind.PageExtensionObjectList: return "PageExtensions";
                case ALSymbolKind.TableExtensionObjectList: return "TableExtensions";
                case ALSymbolKind.ProfileObjectList: return "Profiles";
                case ALSymbolKind.PageCustomizationObjectList: return "PageCustomizations";
                case ALSymbolKind.EnumObjectList: return "Enums";
                case ALSymbolKind.DotNetPackageList: return "DotNetPackages";
                case ALSymbolKind.EnumTypeList: return "Enums";
                case ALSymbolKind.EnumExtensionTypeList: return "EnumExtensions";
                case ALSymbolKind.InterfaceObjectList: return "Interfaces";
                case ALSymbolKind.ReportExtensionObjectList: return "ReportExtensions";
                case ALSymbolKind.PermissionSetList: return "PermissionSets";
                case ALSymbolKind.PermissionSetExtensionList: return "PermissionSetExtensions";
            }
            //throw new Exception("Unsupported enum value " + value.ToString());
            return value.ToString();
        }

        public static ALSymbolKind ToGroupSymbolKind(this ALSymbolKind kind)
        {
            switch (kind)
            {
                case ALSymbolKind.TableObject: return ALSymbolKind.TableObjectList;
                case ALSymbolKind.TableExtensionObject: return ALSymbolKind.TableExtensionObjectList;
                case ALSymbolKind.PageObject: return ALSymbolKind.PageObjectList;
                case ALSymbolKind.ReportObject: return ALSymbolKind.ReportObjectList;
                case ALSymbolKind.XmlPortObject: return ALSymbolKind.XmlPortObjectList;
                case ALSymbolKind.QueryObject: return ALSymbolKind.QueryObjectList;
                case ALSymbolKind.CodeunitObject: return ALSymbolKind.CodeunitObjectList;
                case ALSymbolKind.ControlAddInObject: return ALSymbolKind.ControlAddInObjectList;
                case ALSymbolKind.PageExtensionObject: return ALSymbolKind.PageExtensionObjectList;
                case ALSymbolKind.ProfileObject: return ALSymbolKind.ProfileObjectList;
                case ALSymbolKind.PageCustomizationObject: return ALSymbolKind.PageCustomizationObjectList;
                case ALSymbolKind.DotNetPackage: return ALSymbolKind.DotNetPackageList;
                case ALSymbolKind.EnumType: return ALSymbolKind.EnumTypeList;
                case ALSymbolKind.EnumExtensionType: return ALSymbolKind.EnumExtensionTypeList;
                case ALSymbolKind.Interface: return ALSymbolKind.InterfaceObjectList;
                case ALSymbolKind.ReportExtensionObject: return ALSymbolKind.ReportExtensionObjectList;
                case ALSymbolKind.PermissionSet: return ALSymbolKind.PermissionSetList;
                case ALSymbolKind.PermissionSetExtension: return ALSymbolKind.PermissionSetExtensionList;
                default: return ALSymbolKind.SymbolGroup;
            }
        }

        public static bool IsObjectDefinition(this ALSymbolKind kind)
        {
            switch (kind)
            {
                case ALSymbolKind.TableObject:
                case ALSymbolKind.TableExtensionObject:
                case ALSymbolKind.PageObject:
                case ALSymbolKind.ReportObject:
                case ALSymbolKind.XmlPortObject:
                case ALSymbolKind.QueryObject:
                case ALSymbolKind.CodeunitObject:
                case ALSymbolKind.ControlAddInObject:
                case ALSymbolKind.PageExtensionObject:
                case ALSymbolKind.ProfileObject:
                case ALSymbolKind.PageCustomizationObject:
                case ALSymbolKind.DotNetPackage:
                case ALSymbolKind.EnumType:
                case ALSymbolKind.EnumExtensionType:
                case ALSymbolKind.Interface:
                case ALSymbolKind.ReportExtensionObject:
                case ALSymbolKind.PermissionSet:
                case ALSymbolKind.PermissionSetExtension:
                case ALSymbolKind.Entitlement:
                    return true;
            }
            return false;
        }

        public static bool ServerDefinitionAvailable(this ALSymbolKind kind)
        {
            switch (kind)
            {
                case ALSymbolKind.TableObject:
                case ALSymbolKind.CodeunitObject:
                case ALSymbolKind.PageObject:
                case ALSymbolKind.ReportObject:
                case ALSymbolKind.QueryObject:
                case ALSymbolKind.XmlPortObject:
                case ALSymbolKind.ControlAddInObject:
                case ALSymbolKind.EnumType:
                case ALSymbolKind.Interface:
                    return true;
            }
            return false;
        }

        public static string ToObjectTypeName(this ALSymbolKind kind)
        {
            switch (kind)
            {
                case ALSymbolKind.TableObject:
                    return "Table";
                case ALSymbolKind.CodeunitObject:
                    return "Codeunit";
                case ALSymbolKind.PageObject:
                    return "Page";
                case ALSymbolKind.ReportObject:
                    return "Report";
                case ALSymbolKind.QueryObject:
                    return "Query";
                case ALSymbolKind.XmlPortObject:
                    return "XmlPort";
                case ALSymbolKind.ControlAddInObject:
                    return "ControlAddIn";
                case ALSymbolKind.EnumType:
                    return "Enum";
                case ALSymbolKind.Interface:
                    return "Interface";
                case ALSymbolKind.PageExtensionObject: 
                    return "PageExtension";
                case ALSymbolKind.TableExtensionObject: 
                    return "TableExtension";
                case ALSymbolKind.ProfileObject: 
                    return "Profile";
                case ALSymbolKind.PageCustomizationObject: 
                    return "PageCustomization";
                case ALSymbolKind.DotNetPackage: 
                    return "DotNetPackage";
                case ALSymbolKind.EnumExtensionType: 
                    return "EnumExtension";
                case ALSymbolKind.ReportExtensionObject: 
                    return "ReportExtension";
                case ALSymbolKind.PermissionSet: 
                    return "PermissionSet";
                case ALSymbolKind.PermissionSetExtension: 
                    return "PermissionSetExtension";
            }
            return null;
        }

        public static string ToVariableTypeName(this ALSymbolKind kind)
        {
            switch (kind)
            {
                case ALSymbolKind.TableObject:
                    return "Record";
                case ALSymbolKind.CodeunitObject:
                    return "Codeunit";
                case ALSymbolKind.PageObject:
                    return "Page";
                case ALSymbolKind.ReportObject:
                    return "Report";
                case ALSymbolKind.QueryObject:
                    return "Query";
                case ALSymbolKind.XmlPortObject:
                    return "XmlPort";
                case ALSymbolKind.EnumType:
                    return "Enum";
                case ALSymbolKind.Interface:
                    return "Interface";
            }
            return null;
        }

        public static ALSymbolKind FromObjectTypeName(string name)
        {
            name = name.ToLower();
            switch (name)
            {
                case "table":
                    return ALSymbolKind.TableObject;
                case "page":
                    return ALSymbolKind.PageObject;
                case "report":
                    return ALSymbolKind.ReportObject;
                case "xmlport":
                    return ALSymbolKind.XmlPortObject;
                case "query":
                    return ALSymbolKind.QueryObject;
                case "codeunit":
                    return ALSymbolKind.CodeunitObject;
                case "controladdin":
                    return ALSymbolKind.ControlAddInObject;
                case "pageextension":
                    return ALSymbolKind.PageExtensionObject;
                case "tableextension":
                    return ALSymbolKind.TableExtensionObject;
                case "profile":
                    return ALSymbolKind.ProfileObject;
                case "pagecustomization":
                    return ALSymbolKind.PageCustomizationObject;
                case "dotnetpackage":
                    return ALSymbolKind.DotNetPackage;
                case "enum":
                    return ALSymbolKind.EnumType;
                case "enumextension": 
                    return ALSymbolKind.EnumExtensionType;
                case "interface": 
                    return ALSymbolKind.Interface;
                case "reportextension": 
                    return ALSymbolKind.ReportExtensionObject;
                case "permissionset": 
                    return ALSymbolKind.PermissionSet;
                case "permissionsetextension": 
                    return ALSymbolKind.PermissionSetExtension;
            }
            return ALSymbolKind.Undefined;
        }

    }
}
