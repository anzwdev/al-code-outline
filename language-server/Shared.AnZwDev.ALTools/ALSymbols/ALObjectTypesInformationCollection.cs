using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AnZwDev.ALTools.ALSymbols
{
    public static class ALObjectTypesInformationCollection
    {

        private static List<ALObjectTypeInformation> _objectTypes = null;
        private static Dictionary<ALObjectType, ALObjectTypeInformation> _objectTypesByObjectType = null;
        private static Dictionary<ALSymbolKind, ALObjectTypeInformation> _objectTypesBySymbolKind = null;
        private static Dictionary<string, ALObjectTypeInformation> _objectTypesByTypeName = null;
        private static Dictionary<string, ALObjectTypeInformation> _objectTypesByVariableTypeName = null;
        private static ReadOnlyCollection<ALObjectTypeInformation> _readOnlyObjectTypes = null;

        public static ReadOnlyCollection<ALObjectTypeInformation> ObjectTypes
        {
            get
            {
                Initialize();
                return _readOnlyObjectTypes;
            }
        }

        private static void Initialize()
        {
            if (_objectTypes != null)
                return;

            _objectTypes = new List<ALObjectTypeInformation>();
            _readOnlyObjectTypes = new ReadOnlyCollection<ALObjectTypeInformation>(_objectTypes);
            _objectTypesByObjectType = new Dictionary<ALObjectType, ALObjectTypeInformation>();
            _objectTypesBySymbolKind = new Dictionary<ALSymbolKind, ALObjectTypeInformation>();
            _objectTypesByTypeName = new Dictionary<string, ALObjectTypeInformation>();
            _objectTypesByVariableTypeName = new Dictionary<string, ALObjectTypeInformation>();

            Add(new ALObjectTypeInformation(ALObjectType.Table, ALSymbolKind.TableObject, ALSymbolKind.TableObjectList, "Table", "Record", true));
            Add(new ALObjectTypeInformation(ALObjectType.Page, ALSymbolKind.PageObject, ALSymbolKind.PageObjectList, "Page", "Page", true));
            Add(new ALObjectTypeInformation(ALObjectType.Report, ALSymbolKind.ReportObject, ALSymbolKind.ReportObjectList, "Report", "Report", true));
            Add(new ALObjectTypeInformation(ALObjectType.XmlPort, ALSymbolKind.XmlPortObject, ALSymbolKind.XmlPortObjectList, "XmlPort", "XmlPort", true));
            Add(new ALObjectTypeInformation(ALObjectType.Query, ALSymbolKind.QueryObject, ALSymbolKind.QueryObjectList, "Query", "Query", true));
            Add(new ALObjectTypeInformation(ALObjectType.Codeunit, ALSymbolKind.CodeunitObject, ALSymbolKind.CodeunitObjectList, "Codeunit", "Codeunit", true));
            Add(new ALObjectTypeInformation(ALObjectType.ControlAddIn, ALSymbolKind.ControlAddInObject, ALSymbolKind.ControlAddInObjectList, "ControlAddIn", null, true));
            Add(new ALObjectTypeInformation(ALObjectType.PageExtension, ALSymbolKind.PageExtensionObject, ALSymbolKind.PageExtensionObjectList, "PageExtension", null, false));
            Add(new ALObjectTypeInformation(ALObjectType.TableExtension, ALSymbolKind.TableExtensionObject, ALSymbolKind.TableExtensionObjectList, "TableExtension", null, false));
            Add(new ALObjectTypeInformation(ALObjectType.Profile, ALSymbolKind.ProfileObject, ALSymbolKind.ProfileObjectList, "Profile", null, false));
            Add(new ALObjectTypeInformation(ALObjectType.PageCustomization, ALSymbolKind.PageCustomizationObject, ALSymbolKind.PageCustomizationObjectList, "PageCustomization", null, false));
            Add(new ALObjectTypeInformation(ALObjectType.DotNetPackage, ALSymbolKind.DotNetPackage, ALSymbolKind.DotNetPackageList, "DotNetPackage", null, false));
            Add(new ALObjectTypeInformation(ALObjectType.EnumType, ALSymbolKind.EnumType, ALSymbolKind.EnumTypeList, "Enum", "Enum", true));
            Add(new ALObjectTypeInformation(ALObjectType.EnumExtensionType, ALSymbolKind.EnumExtensionType, ALSymbolKind.EnumExtensionTypeList, "EnumExtension", null, false));
            Add(new ALObjectTypeInformation(ALObjectType.Interface, ALSymbolKind.Interface, ALSymbolKind.InterfaceObjectList, "Interface", "Interface", true));
            Add(new ALObjectTypeInformation(ALObjectType.ReportExtension, ALSymbolKind.ReportExtensionObject, ALSymbolKind.ReportExtensionObjectList, "ReportExtension", null, false));
            Add(new ALObjectTypeInformation(ALObjectType.PermissionSet, ALSymbolKind.PermissionSet, ALSymbolKind.PermissionSetList, "PermissionSet", null, true));
            Add(new ALObjectTypeInformation(ALObjectType.PermissionSetExtension, ALSymbolKind.PermissionSetExtension, ALSymbolKind.PermissionSetExtensionList, "PermissionSetExtension", null, false));
            Add(new ALObjectTypeInformation(ALObjectType.Entitlement, ALSymbolKind.Entitlement, ALSymbolKind.EntitlementList, "Entitlement", null, false));
        }

        private static void Add(ALObjectTypeInformation mapping)
        {
            _objectTypes.Add(mapping);
            _objectTypesByObjectType.Add(mapping.ALObjectType, mapping);
            _objectTypesBySymbolKind.Add(mapping.ALSymbolKind, mapping);
            _objectTypesByTypeName.Add(mapping.ObjectTypeName.ToLower(), mapping);
            if (!String.IsNullOrWhiteSpace(mapping.VariableTypeName))
                _objectTypesByVariableTypeName.Add(mapping.VariableTypeName.ToLower(), mapping);
        }

        public static ALObjectTypeInformation Get(ALObjectType type)
        {
            if (type == ALObjectType.None)
                return null;
            Initialize();
            return _objectTypesByObjectType[type];
        }

        public static ALObjectTypeInformation Get(ALSymbolKind symbolKind)
        {
            Initialize();
            if (_objectTypesBySymbolKind.ContainsKey(symbolKind))
                return _objectTypesBySymbolKind[symbolKind];
            return null;
        }

        public static ALObjectTypeInformation Get(string objectTypeName)
        {
            if (objectTypeName == null)
                return null;
            Initialize();
            objectTypeName = objectTypeName.ToLower();
            if (_objectTypesByTypeName.ContainsKey(objectTypeName))
                return _objectTypesByTypeName[objectTypeName];
            return null;
        }

        public static ALObjectTypeInformation GetForVariableTypeName(string variableTypeName)
        {
            if (variableTypeName == null)
                return null;
            Initialize();
            variableTypeName = variableTypeName.ToLower();
            if (_objectTypesByVariableTypeName.ContainsKey(variableTypeName))
                return _objectTypesByVariableTypeName[variableTypeName];
            return null;
        }

    }
}
