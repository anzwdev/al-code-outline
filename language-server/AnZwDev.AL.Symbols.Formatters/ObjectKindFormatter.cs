using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Symbols.Formatters
{
    public static class ObjectKindFormatter
    {

        public static string FormatAsObjectTypeName(ObjectKind objectKind)
        {
            switch (objectKind)
            {
                case ObjectKind.Table:
                    return "Table";
                case ObjectKind.Codeunit:
                    return "Codeunit";
                case ObjectKind.Page:
                    return "Page";
                case ObjectKind.Report:
                    return "Report";
                case ObjectKind.Query:
                    return "Query";
                case ObjectKind.XmlPort:
                    return "XmlPort";
                case ObjectKind.ControlAddIn:
                    return "ControlAddIn";
                case ObjectKind.EnumType:
                    return "Enum";
                case ObjectKind.Interface:
                    return "Interface";
                case ObjectKind.PageExtension:
                    return "PageExtension";
                case ObjectKind.TableExtension:
                    return "TableExtension";
                case ObjectKind.Profile:
                    return "Profile";
                case ObjectKind.ProfileExtension:
                    return "ProfileExtension";
                case ObjectKind.PageCustomization:
                    return "PageCustomization";
                case ObjectKind.DotNetPackage:
                    return "DotNetPackage";
                case ObjectKind.EnumExtensionType:
                    return "EnumExtension";
                case ObjectKind.ReportExtension:
                    return "ReportExtension";
                case ObjectKind.PermissionSet:
                    return "PermissionSet";
                case ObjectKind.PermissionSetExtension:
                    return "PermissionSetExtension";
                case ObjectKind.Entitlement:
                    return "Entitlement";
            }

            return String.Empty;
        }


    }
}


    

