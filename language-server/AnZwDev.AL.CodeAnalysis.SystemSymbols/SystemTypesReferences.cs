using AnZwDev.AL.Symbols;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.CodeAnalysis.SystemSymbols
{
    public static class SystemTypesReferences
    {

        private static Dictionary<NavTypeKind, SystemTypeSymbol>? _types;
        public static Dictionary<NavTypeKind, SystemTypeSymbol> Types
        {
            get
            {
                if (_types == null)
                    CreateTypes();
                return _types!;
            }
        }

        private static void CreateTypes()
        {
            _types = new Dictionary<NavTypeKind, SystemTypeSymbol>();

            AddType(
                new SystemTypeSymbol(
                    NavTypeKind.ErrorInfo,
                    new SystemMethodSymbol(
                        "AddAction",
                        new SystemMethodParameterSymbol(1, ObjectKind.Codeunit)),
                    new SystemMethodSymbol(
                        "PageNo",
                        new SystemMethodParameterSymbol(0, ObjectKind.Page)),
                    new SystemMethodSymbol(
                        "TableId",
                        new SystemMethodParameterSymbol(0, ObjectKind.Table))));

            AddType(
                new SystemTypeSymbol(
                    NavTypeKind.DataTransfer,
                    new SystemMethodSymbol(
                        "SetTables",
                        new SystemMethodParameterSymbol(0, ObjectKind.Table),
                        new SystemMethodParameterSymbol(1, ObjectKind.Table))));

            AddType(
                new SystemTypeSymbol(
                    NavTypeKind.FilterPageBuilder,
                    new SystemMethodSymbol(
                        "AddTable",
                        new SystemMethodParameterSymbol(1, ObjectKind.Table))));

            AddType(
                new SystemTypeSymbol(
                    NavTypeKind.Notification,
                    new SystemMethodSymbol(
                        "AddAction",
                        new SystemMethodParameterSymbol(1, ObjectKind.Codeunit))));
        }

        private static void AddType(SystemTypeSymbol systemType)
        {
            _types!.Add(systemType.NavTypeKind, systemType);
        }

        public static SystemTypeSymbol? GetSystemType(NavTypeKind navTypeKind)
        {
            if (Types.ContainsKey(navTypeKind))
                return Types[navTypeKind];
            return null;
        }

        public static SystemMethodSymbol? GetSystemTypeMethod(NavTypeKind navTypeKind, string methodName)
        {
            return GetSystemType(navTypeKind)?.GetMethod(methodName);
        }

    }
}
