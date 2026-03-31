using AnZwDev.AL.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews.SymbolFactories.PropertySetters
{
    internal class TreeViewNodeObjectWithIdNameSetter : TreeViewNodeIdentifierNameNameSetter
    {

        public override void SetName(SyntaxTreeSymbolsTreeViewNode node, NameSyntax? nameNode)
        {
            base.SetName(node, nameNode);
            node.FullName = node.Kind.ToDescriptionString() + " " + node.Id.ToString() + " " + ALLiteralFormatter.GetName(node.Name);
        }

    }
}
