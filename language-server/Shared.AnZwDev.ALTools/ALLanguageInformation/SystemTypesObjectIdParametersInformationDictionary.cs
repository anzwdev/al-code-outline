using AnZwDev.ALTools.ALSymbols.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALLanguageInformation
{
    internal static class SystemTypesObjectIdParametersInformationDictionary
    {

        private static Dictionary<ConvertedNavTypeKind, SystemTypeObjectIdParametersInformation> _types = null;
        public static Dictionary<ConvertedNavTypeKind, SystemTypeObjectIdParametersInformation> Types
        {
            get
            {
                if (_types == null)
                    CreateTypes();
                return _types;
            }
        }

        private static void CreateTypes()
        {
            _types = new Dictionary<ConvertedNavTypeKind, SystemTypeObjectIdParametersInformation>();

            AddType(
                new SystemTypeObjectIdParametersInformation(
                    ConvertedNavTypeKind.ErrorInfo,
                    new MethodObjectIdParametersInformation(
                        "AddAction",
                        new ObjectIdParameterInformation(1, "codeunit")),
                    new MethodObjectIdParametersInformation(
                        "PageNo",
                        new ObjectIdParameterInformation(0, "page")),
                    new MethodObjectIdParametersInformation(
                        "TableId",
                        new ObjectIdParameterInformation(0, "table"))));

            AddType(
                new SystemTypeObjectIdParametersInformation(
                    ConvertedNavTypeKind.DataTransfer,
                    new MethodObjectIdParametersInformation(
                        "SetTables",
                        new ObjectIdParameterInformation(0, "table"),
                        new ObjectIdParameterInformation(1, "table"))));

            AddType(
                new SystemTypeObjectIdParametersInformation(
                    ConvertedNavTypeKind.FilterPageBuilder,
                    new MethodObjectIdParametersInformation(
                        "AddTable",
                        new ObjectIdParameterInformation(1, "table"))));

            AddType(
                new SystemTypeObjectIdParametersInformation(
                    ConvertedNavTypeKind.Notification,
                    new MethodObjectIdParametersInformation(
                        "AddAction",
                        new ObjectIdParameterInformation(1, "codeunit"))));
        }

        private static void AddType(SystemTypeObjectIdParametersInformation systemType)
        {
            _types.Add(systemType.NavTypeKind, systemType);
        }

        public static SystemTypeObjectIdParametersInformation GetSystemType(ConvertedNavTypeKind navTypeKind)
        {
            if (Types.ContainsKey(navTypeKind))
                return Types[navTypeKind];
            return null;
        }

        public static MethodObjectIdParametersInformation GetSystemTypeMethod(ConvertedNavTypeKind navTypeKind, string methodName)
        {
            var systemType = GetSystemType(navTypeKind);
            if (systemType != null)
                return systemType.GetMethod(methodName);
            return null;
        }

    }
}
