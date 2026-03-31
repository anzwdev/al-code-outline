using AnZwDev.AL.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews.SymbolFactories.PropertySetters
{
    internal class TreeViewNodeKindNameSetter : TreeViewNodeNameSetter
    {

        public override void SetName(SyntaxTreeSymbolsTreeViewNode node, NameSyntax? nameNode)
        {
            node.Name = node.Kind.ToDescriptionString();
        }

    }
}
