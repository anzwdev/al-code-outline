using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences.Extensions
{
    public static class ALAppPermissionObjectTypeExtensions
    {

        public static ALAppPermissionObjectType FromString(string source)
        {
            if (source != null)
            {
                source = source.ToLower();
                switch (source)
                {
                    case "tabledata":
                        return ALAppPermissionObjectType.TableData;
                    case "table":
                        return ALAppPermissionObjectType.Table;
                    case "report":
                        return ALAppPermissionObjectType.Report;
                    case "codeunit":
                        return ALAppPermissionObjectType.Codeunit;
                    case "xmlport":
                        return ALAppPermissionObjectType.XmlPort;
                    case "page":
                        return ALAppPermissionObjectType.Page;
                    case "query":
                        return ALAppPermissionObjectType.Query;
                    case "system":
                        return ALAppPermissionObjectType.System;
                }
            }
            return ALAppPermissionObjectType.Undefined;
        }

        public static ALAppPermissionObjectType FromSymbolKind(ConvertedSymbolKind kind)
        {
            switch (kind)
            {
                case ConvertedSymbolKind.Record:
                case ConvertedSymbolKind.Table:
                    return ALAppPermissionObjectType.Table;
                case ConvertedSymbolKind.Report:
                    return ALAppPermissionObjectType.Report;
                case ConvertedSymbolKind.Codeunit:
                    return ALAppPermissionObjectType.Codeunit;
                case ConvertedSymbolKind.XmlPort:
                    return ALAppPermissionObjectType.XmlPort;
                case ConvertedSymbolKind.Page:
                    return ALAppPermissionObjectType.Page;
                case ConvertedSymbolKind.Query:
                    return ALAppPermissionObjectType.Query;
            }
            return ALAppPermissionObjectType.Undefined;
        }

        public static ALAppPermissionObjectType FromSymbolKind(SymbolKind kind)
        {
            return FromSymbolKind(kind.ConvertToLocalType());
        }

    }
}
