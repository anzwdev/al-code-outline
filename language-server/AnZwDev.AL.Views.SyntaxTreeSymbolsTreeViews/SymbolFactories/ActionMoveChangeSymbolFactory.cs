using AnZwDev.AL.Syntax;
using AnZwDev.AL.CodeAnalysis.Extensions;
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
    internal static class ActionMoveChangeSymbolFactory
    {
        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(ActionMoveChangeSyntax node, SyntaxTreeSymbolsTreeViewNode? parentNode)
        {
            return SyntaxNodeSymbolFactory.CreateSymbol(
                node, node.Anchor, 
                ALSyntaxNodeKind.ActionMoveChange,
                TreeViewNodeNameSetters.KindWithIdentifierName);
        }
    }
}
