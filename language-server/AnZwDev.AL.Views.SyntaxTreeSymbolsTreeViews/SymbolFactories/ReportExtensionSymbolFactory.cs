using AnZwDev.AL.Syntax;
using AnZwDev.AL.CodeAnalysis.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews.SymbolFactories
{
    internal static class ReportExtensionSymbolFactory
    {
        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(ReportExtensionSyntax node, SyntaxTreeSymbolsTreeViewNode? parentNode)
        {
            return ApplicationObjectExtensionSymbolFactory.CreateSymbol(node, parentNode, ALSyntaxNodeKind.ReportExtensionObject);
        }
    }
}
