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
    internal static class EventTriggerDeclarationSymbolFactory
    {
        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(EventTriggerDeclarationSyntax node, SyntaxTreeSymbolsTreeViewNode? parentNode)
        {
            return MethodOrTriggerDeclarationSymbolFactory.CreateSymbol(node, parentNode, ALSyntaxNodeKind.EventTriggerDeclaration);
        }
    }
}
