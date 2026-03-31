using AnZwDev.AL.CodeAnalysis.Extensions;
using AnZwDev.AL.Syntax;
using AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews.SymbolFactories.Formatters;
using AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews.SymbolFactories.PropertySetters;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews.SymbolFactories
{
    internal static class FieldSymbolFactory
    {
        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(FieldSyntax node, SyntaxTreeSymbolsTreeViewNode? parentNode)
        {
            var symbol = SyntaxNodeSymbolFactory.CreateSymbol(
                node, node.Name, 
                ALSyntaxNodeKind.Field, 
                TreeViewNodeNameSetters.IdentifierName,
                node.OpenBraceToken, node.CloseBraceToken);

            if (Int32.TryParse(node.No.ToString(), out int id))
                symbol.Id = id;

            symbol.FullName = ALLiteralFormatter.GetName(symbol.Name) + ": " + DataTypeFormatter.GetCode(node.Type);

            var enabled = node.GetBoolPropertyValue("Enabled", true);
            var obsoleteState = node.GetDecodedNamePropertyValue("ObsoleteState");

            if (!enabled)
            {
                symbol.Subtype = "Disabled";
                symbol.FullName = symbol.FullName + " (Disabled)";
            }
            else if (!String.IsNullOrWhiteSpace(obsoleteState))
            {
                if (obsoleteState.Equals("Pending", StringComparison.OrdinalIgnoreCase))
                {
                    symbol.Subtype = "ObsoletePending";
                    symbol.FullName = symbol.FullName + " (Obsolete-Pending)";
                }
                else if (obsoleteState.Equals("Removed", StringComparison.OrdinalIgnoreCase))
                {
                    symbol.Subtype = "ObsoleteRemoved";
                    symbol.FullName = symbol.FullName + " (Obsolete-Removed)";
                }
            }

            return symbol;
        }
    }
}
