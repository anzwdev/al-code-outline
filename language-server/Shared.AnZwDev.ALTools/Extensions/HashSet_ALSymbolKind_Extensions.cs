using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Extensions
{
    public static class HashSet_ALSymbolKind_Extensions
    {

        public static void AddObjectTypes(this HashSet<ALSymbolKind> symbolKindSet, bool addTables, bool addPages, bool addReports, bool addXmlPorts,
            bool addQueries, bool addCodeunits, bool addControlAddIns, bool addPageExtensions, bool addTableExtensions, bool addProfiles,
            bool addPageCustomizations, bool addDotNetPackages, bool addEnumTypes, bool addEnumExtensionTypes, bool addInterfaces, 
            bool addReportExtensions, bool addPermissionSets, bool addPermissionSetExtensions)
        {
            if (addTables) symbolKindSet.Add(ALSymbolKind.TableObject);
            if (addPages) symbolKindSet.Add(ALSymbolKind.PageObject);
            if (addReports) symbolKindSet.Add(ALSymbolKind.ReportObject);
            if (addXmlPorts) symbolKindSet.Add(ALSymbolKind.XmlPortObject);
            if (addQueries) symbolKindSet.Add(ALSymbolKind.QueryObject);
            if (addCodeunits) symbolKindSet.Add(ALSymbolKind.CodeunitObject);
            if (addControlAddIns) symbolKindSet.Add(ALSymbolKind.ControlAddInObject);
            if (addPageExtensions) symbolKindSet.Add(ALSymbolKind.PageExtensionObject);
            if (addTableExtensions) symbolKindSet.Add(ALSymbolKind.TableExtensionObject);
            if (addProfiles) symbolKindSet.Add(ALSymbolKind.ProfileObject);
            if (addPageCustomizations) symbolKindSet.Add(ALSymbolKind.PageCustomizationObject);
            if (addDotNetPackages) symbolKindSet.Add(ALSymbolKind.DotNetPackage);
            if (addEnumTypes) symbolKindSet.Add(ALSymbolKind.EnumType);
            if (addEnumExtensionTypes) symbolKindSet.Add(ALSymbolKind.EnumExtensionType);
            if (addInterfaces) symbolKindSet.Add(ALSymbolKind.Interface);
            if (addReportExtensions) symbolKindSet.Add(ALSymbolKind.ReportExtensionObject);
            if (addPermissionSets) symbolKindSet.Add(ALSymbolKind.PermissionSet);
            if (addPermissionSetExtensions) symbolKindSet.Add(ALSymbolKind.PermissionSetExtension);
        }

        public static void AddAllObjectTypes(this HashSet<ALSymbolKind> symbolKindSet)
        {
            symbolKindSet.AddObjectTypes(true, true, true, true, true,
                true, true, true, true, true,
                true, true, true, true, true,
                true, true, true);
        }

        public static void AddObjectsWithPermissions(this HashSet<ALSymbolKind> symbolKindSet)
        {
            symbolKindSet.Add(ALSymbolKind.TableObject);
            symbolKindSet.Add(ALSymbolKind.PageObject);
            symbolKindSet.Add(ALSymbolKind.ReportObject);
            symbolKindSet.Add(ALSymbolKind.XmlPortObject);
            symbolKindSet.Add(ALSymbolKind.QueryObject);
            symbolKindSet.Add(ALSymbolKind.CodeunitObject);
        }

    }
}
