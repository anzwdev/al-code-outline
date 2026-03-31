using AnZwDev.AL.Syntax;
using AnZwDev.AL.CodeAnalysis.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews.SymbolFactories
{
    internal static class GlobalVarSectionSymbolFactory
    {
        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(GlobalVarSectionSyntax node, SyntaxTreeSymbolsTreeViewNode? parentNode)
        {
            var symbol = VarSectionBaseSymbolFactory.CreateSymbol(node, parentNode, ALSyntaxNodeKind.GlobalVarSection);

            (var hasChildNodes, var span) = node.GetChildNodesFullSpan();
            if (hasChildNodes)
                symbol.ContentRange = node.SyntaxTree.GetLineRange(span);

            var accessModifier = node.AccessModifier.ToString()?.Trim();
            if ((accessModifier != null) && (accessModifier.Equals("protected", StringComparison.OrdinalIgnoreCase)))
            {
                symbol.Access = ALSyntaxNodeAccessModifier.Protected;
                symbol.FullName = "protected var";
            }

            return symbol;
        }
    }
}
