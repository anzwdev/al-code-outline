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
    internal static class VarSectionSymbolFactory
    {
        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(VarSectionSyntax node, SyntaxTreeSymbolsTreeViewNode? parentNode)
        {
            return VarSectionBaseSymbolFactory.CreateSymbol(node, parentNode, ALSyntaxNodeKind.VarSection);
        }
    }
}
