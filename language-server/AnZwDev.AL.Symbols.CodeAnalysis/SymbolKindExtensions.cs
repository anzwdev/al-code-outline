using AnZwDev.AL.Symbols;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.CodeAnalysis
{
    public static class SymbolKindExtensions
    {

        public static ObjectKind ToObjectKind(this SymbolKind kind)
        {
            return kind switch
            {
                SymbolKind.Record => ObjectKind.Table,
                SymbolKind.Table => ObjectKind.Table,
                SymbolKind.Report => ObjectKind.Report,
                SymbolKind.Codeunit => ObjectKind.Codeunit,
                SymbolKind.Page => ObjectKind.Page,
                SymbolKind.Query => ObjectKind.Query,
                SymbolKind.XmlPort => ObjectKind.XmlPort,
                SymbolKind.Enum => ObjectKind.EnumType,
                SymbolKind.EnumExtension => ObjectKind.EnumExtensionType,
                SymbolKind.PageExtension => ObjectKind.PageExtension,
                SymbolKind.TableExtension => ObjectKind.TableExtension,
                SymbolKind.ControlAddIn => ObjectKind.ControlAddIn,
                SymbolKind.Profile => ObjectKind.Profile,
                SymbolKind.PermissionSet => ObjectKind.PermissionSet,
                SymbolKind.DotNetPackage => ObjectKind.DotNetPackage,
                SymbolKind.Interface => ObjectKind.Interface,

                _ => ObjectKind.Unknown,
            };
        }


    }
}
