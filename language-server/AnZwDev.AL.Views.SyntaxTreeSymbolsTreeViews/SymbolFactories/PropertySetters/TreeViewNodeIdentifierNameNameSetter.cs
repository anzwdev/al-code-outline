using AnZwDev.AL.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews.SymbolFactories.PropertySetters
{
    internal class TreeViewNodeIdentifierNameNameSetter : TreeViewNodeNameSetter
    {

        public override void SetName(SyntaxTreeSymbolsTreeViewNode node, NameSyntax? nameNode)
        {
            if (nameNode is IdentifierNameSyntax identifierNameSyntax)
                node.Name = ALLiteralParser.ParseName((identifierNameSyntax.Identifier.Text) ?? String.Empty);
            else
                node.Name = ALLiteralParser.ParseName((nameNode?.ToString()) ?? String.Empty);
        }

    }
}
